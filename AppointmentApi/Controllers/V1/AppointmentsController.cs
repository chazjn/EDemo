using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApi.Db;
using AppointmentApi.Db.Models;
using AppointmentApi.Dto;
using AppointmentApi.Validation;
using EmailNotificationSystem;
using EquipmentAvailabilty;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IValidatorFactory _validatorFactory;
        private readonly IAppointmentsRepository _appointmentsRepository;
        private readonly IEquipmentAvailabilityService _equipmentAvailabilityService;
        private readonly ISmtpClient _smtpClient;

        public AppointmentsController(IValidatorFactory validatorFactory, IAppointmentsRepository appointmentsRepository, IEquipmentAvailabilityService equipmentAvailabilityService, ISmtpClient smtpClient)
        {
            _validatorFactory = validatorFactory;
            _appointmentsRepository = appointmentsRepository;
            _equipmentAvailabilityService = equipmentAvailabilityService;
            _smtpClient = smtpClient;
        }

        [HttpGet]
        public async Task<ActionResult<IList<AppointmentDto>>> Get()
        {
            var appointmentList = await _appointmentsRepository.GetAppointmentsByDateAsync(DateTime.Now.Date);
            var appointmentDtoList = appointmentList.Select(x => new AppointmentDto
            {
                PatientId = x.PatientId,
                DateTime = x.DateTime
            });

            return Ok(appointmentDtoList);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(AppointmentDto appointmentDto)
        {
            var validator = _validatorFactory.Build(Validator.Create);
            var errors = validator.Validate(appointmentDto);

            if (errors.Count > 0)
                return BadRequest(errors);

            var reserveRequest = _equipmentAvailabilityService.ReserveEquipment(appointmentDto.DateTime);
            if(reserveRequest.Successful == false)
                return BadRequest("Equipment unavailable");

            await _appointmentsRepository.CreateAppointmentAsync(appointmentDto, reserveRequest.EquipmentAvailability.EquipmentId);
            var patient = await _appointmentsRepository.GetPatientAsync(appointmentDto.PatientId);

            await _smtpClient.SendAsync(new Email
            {
                To = patient.EmailAddress,
                Subject = "Successful booking",
                Body = "Successful booking"
            });

            return Ok();
        }

        [HttpPost]
        [Route("Change")]
        public async Task<IActionResult> Change(AppointmentChangeDto appointmentDto)
        {
            var validator = _validatorFactory.Build(Validator.Change);
            var errors = validator.Validate(appointmentDto);

            if (errors.Count > 0)
                return BadRequest(errors);

            var reserveRequest = _equipmentAvailabilityService.ReserveEquipment(appointmentDto.NewDateTime);
            if (reserveRequest.Successful == false)
                return BadRequest("Equipment unavailable");

            _equipmentAvailabilityService.UnreserveEquipment(appointmentDto.DateTime);
            await _appointmentsRepository.ChangeAppointmentAsync(appointmentDto);

            return Ok();
        }

        [HttpPost]
        [Route("Cancel")]
        public async Task<IActionResult> Cancel(AppointmentDto appointmentDto)
        {
            var validator = _validatorFactory.Build(Validator.Cancel);
            var errors = validator.Validate(appointmentDto);

            if (errors.Count > 0)
                return BadRequest(errors);

            await _appointmentsRepository.CancelAppointmentAsync(appointmentDto);
            _equipmentAvailabilityService.UnreserveEquipment(appointmentDto.DateTime);

            return Ok();
        }
    }
}
