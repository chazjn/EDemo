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
        public async Task<IActionResult> Create(AppointmentDto appointmentDto)
        {
            var validator = _validatorFactory.Build(appointmentDto);
            var errors = validator.Validate(appointmentDto);
            if (errors.Count > 0)
                return BadRequest(errors);

            var patient = await _appointmentsRepository.GetPatientAsync(appointmentDto.PatientId);
            if (patient == null)
                return BadRequest(new[] { new PatientDoesNotExist(appointmentDto.PatientId) });

            var appointment = await _appointmentsRepository.GetAppointmentAsync(appointmentDto.PatientId, appointmentDto.DateTime);
            if(appointment != null)
                return BadRequest(new[] { new AppointmentAlreadyExists(appointmentDto.PatientId, appointmentDto.DateTime) });

            var reserveRequest = _equipmentAvailabilityService.ReserveEquipment(appointmentDto.DateTime);
            if(reserveRequest.Successful == false)
                return BadRequest(new[] { new EquipmentUnavailable(appointmentDto.DateTime) });

            await _appointmentsRepository.CreateAppointmentAsync(appointmentDto.PatientId, appointmentDto.DateTime, reserveRequest.EquipmentAvailability.EquipmentId);
            
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
        public async Task<IActionResult> Change(ChangeAppointmentDto appointmentDto)
        {
            var validator = _validatorFactory.Build(appointmentDto);
            var errors = validator.Validate(appointmentDto);
            if (errors.Count > 0)
                return BadRequest(errors);


            var patient = await _appointmentsRepository.GetPatientAsync(appointmentDto.PatientId);
            if (patient == null)
                return BadRequest(new[] { new PatientDoesNotExist(patient.Id) });


            var previousAppointment = await _appointmentsRepository.GetAppointmentAsync(appointmentDto.PatientId, appointmentDto.PreviousDateTime);
            if (previousAppointment == null)
                return BadRequest(new[] { new AppointmentDoesNotExist(appointmentDto.PatientId, appointmentDto.DateTime) });


            var newAppointment = await _appointmentsRepository.GetAppointmentAsync(appointmentDto.PatientId, appointmentDto.DateTime);
            if (newAppointment == null)
                return BadRequest(new[] { new AppointmentAlreadyExists(appointmentDto.PatientId, appointmentDto.DateTime) });


            var reserveRequest = _equipmentAvailabilityService.ReserveEquipment(appointmentDto.DateTime);
            if (reserveRequest.Successful == false)
                return BadRequest(new[] { new EquipmentUnavailable(appointmentDto.DateTime) });


            _equipmentAvailabilityService.UnreserveEquipment(appointmentDto.DateTime);
            await _appointmentsRepository.ChangeAppointmentAsync(appointmentDto.PatientId, appointmentDto.PreviousDateTime, appointmentDto.DateTime);

            return Ok();
        }

        [HttpPost]
        [Route("Cancel")]
        public async Task<IActionResult> Cancel(CancelAppointmentDto appointmentDto)
        {
            var validator = _validatorFactory.Build(appointmentDto);
            var errors = validator.Validate(appointmentDto);
            if (errors.Count > 0)
                return BadRequest(errors);

            var patient = await _appointmentsRepository.GetPatientAsync(appointmentDto.PatientId);
            if (patient == null)
                return BadRequest(new[] { new PatientDoesNotExist(patient.Id) });

            var appointment = await _appointmentsRepository.GetAppointmentAsync(appointmentDto.PatientId, appointmentDto.DateTime);
            if (appointment == null)
                return BadRequest(new[] { new AppointmentDoesNotExist(appointmentDto.PatientId, appointmentDto.DateTime) });

            await _appointmentsRepository.CancelAppointmentAsync(appointmentDto.PatientId, appointment.DateTime);
            _equipmentAvailabilityService.UnreserveEquipment(appointmentDto.DateTime);

            return Ok();
        }
    }
}
