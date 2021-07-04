using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApi.Db;
using AppointmentApi.Db.Models;
using AppointmentApi.Dto;
using AppointmentApi.Validation;
using AppointmentApi.Validation.ValidationErrors;
using EmailNotificationSystem;
using EquipmentAvailabiltySystem;
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
        public async Task<IActionResult> Create(AppointmentDto dto)
        {
            var validator = _validatorFactory.Build(dto);
            var validationErrors = validator.Validate(dto);
            if (validationErrors.Count > 0)
                return BadRequest(validationErrors);

            var reserveRequest = _equipmentAvailabilityService.ReserveEquipment(dto.DateTime);
            if(reserveRequest.Successful == false)
                return BadRequest(new[] { new EquipmentUnavailable(dto.DateTime) });

            var createErrors = await _appointmentsRepository.TryCreateAppointmentAsync(dto.PatientId, dto.DateTime, reserveRequest.EquipmentAvailability.EquipmentId);
            if(createErrors.Count > 0)
            {
                _equipmentAvailabilityService.UnreserveEquipment(dto.DateTime);
                return BadRequest(createErrors);
            }

            var patient = await _appointmentsRepository.GetPatientAsync(dto.PatientId);

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
        public async Task<IActionResult> Change(ChangeAppointmentDto dto)
        {
            var validator = _validatorFactory.Build(dto);
            var validationErrors = validator.Validate(dto);
            if (validationErrors.Count > 0)
                return BadRequest(validationErrors);

            var reserveRequest = _equipmentAvailabilityService.ReserveEquipment(dto.DateTime);
            if (reserveRequest.Successful == false)
                return BadRequest(new[] { new EquipmentUnavailable(dto.DateTime) });

            var changeErrors = await _appointmentsRepository.TryChangeAppointmentAsync(dto.PatientId, dto.PreviousDateTime, dto.DateTime);
            if (changeErrors.Count > 0)
            {
                _equipmentAvailabilityService.UnreserveEquipment(dto.DateTime);
                return BadRequest(changeErrors);
            }
                
            _equipmentAvailabilityService.UnreserveEquipment(dto.PreviousDateTime);
            
            return Ok();
        }

        [HttpPost]
        [Route("Cancel")]
        public async Task<IActionResult> Cancel(CancelAppointmentDto dto)
        {
            var validator = _validatorFactory.Build(dto);
            var validationErrors = validator.Validate(dto);
            if (validationErrors.Count > 0)
                return BadRequest(validationErrors);

            var cancelErrors = await _appointmentsRepository.TryCancelAppointmentAsync(dto.PatientId, dto.DateTime);
            if(cancelErrors.Count > 0)
                return BadRequest(validationErrors);

            _equipmentAvailabilityService.UnreserveEquipment(dto.DateTime);

            return Ok();
        }
    }
}
