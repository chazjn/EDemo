using System;
using System.Collections.Generic;
using AppointmentApi.Db;
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
        public ActionResult<IList<AppointmentDto>> Get()
        {
            var list = _appointmentsRepository.GetAppointmentsByDate(DateTime.Now.Date);
            return Ok(list);
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(AppointmentDto appointmentDto)
        {
            var validator = _validatorFactory.Build("create");
            var errors = validator.Validate(appointmentDto);

            if (errors.Count > 0)
                return BadRequest(errors);

            var reserveRequest = _equipmentAvailabilityService.ReserveEquipment(appointmentDto.DateTime);
            if(reserveRequest.Successful == false)
                return BadRequest("Equipment unavailable");

            _appointmentsRepository.CreateAppointment(appointmentDto, reserveRequest.EquipmentAvailability.EquipmentId);
            var patient = _appointmentsRepository.GetPatient(appointmentDto.PatientId);

            _smtpClient.Send(new Email
            {
                To = patient.EmailAddress,
                Subject = "Successful booking",
                Body = "Successful booking"
            });

            return Ok();
        }

        [HttpPost]
        [Route("Change")]
        public IActionResult Change(AppointmentChangeDto appointmentDto)
        {
            var validator = _validatorFactory.Build("change");
            var errors = validator.Validate(appointmentDto);

            if (errors.Count > 0)
                return BadRequest(errors);

            var reserveRequest = _equipmentAvailabilityService.ReserveEquipment(appointmentDto.NewDateTime);
            if (reserveRequest.Successful == false)
                return BadRequest("Equipment unavailable");

            _equipmentAvailabilityService.UnreserveEquipment(appointmentDto.DateTime);
            _appointmentsRepository.ChangeAppointment(appointmentDto);

            return Ok();
        }

        [HttpPost]
        [Route("Cancel")]
        public IActionResult Cancel(AppointmentDto appointmentDto)
        {
            var validator = _validatorFactory.Build("cancel");
            var errors = validator.Validate(appointmentDto);

            if (errors.Count > 0)
                return BadRequest(errors);

            _appointmentsRepository.CancelAppointment(appointmentDto);
            _equipmentAvailabilityService.UnreserveEquipment(appointmentDto.DateTime);

            return Ok();
        }
    }
}
