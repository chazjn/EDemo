using System.Collections.Generic;
using AppointmentApi.Dto;
using AppointmentApi.Validation;
using EmailNotificationSystem;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IValidatorFactory _validatorFactory;
        private readonly ISmtpClient _smtpClient;

        public AppointmentsController(IValidatorFactory validatorFactory, ISmtpClient smtpClient)
        {
            _validatorFactory = validatorFactory;
            _smtpClient = smtpClient;
        }

        [HttpGet]
        public ActionResult<IList<AppointmentDto>> Get()
        {
            //var list = _appointmentsRepository.GetAppointmentsByDate(DateTime.Now.Date);
            return Ok("list");
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(AppointmentDto appointmentDto)
        {
            var validator = _validatorFactory.Build("create");
            var errors = validator.Validate(appointmentDto);

            if (errors.Count == 0)
            {
                //book out equipment
                //create appointment
                //send email
                return Ok();
            }
            else
            {
                return BadRequest(errors);
            }
        }

        [HttpPost]
        [Route("Change")]
        public IActionResult Change(AppointmentChangeDto appointmentDto)
        {
            var validator = _validatorFactory.Build("change");
            var errors = validator.Validate(appointmentDto);

            if (errors.Count == 0)
            {
                //update equipment
                //update appointment
                return Ok();
            }
            else
            {
                return BadRequest(errors);
            }
        }

        [HttpPost]
        [Route("Cancel")]
        public IActionResult Cancel(AppointmentChangeDto appointmentDto)
        {
            var validator = _validatorFactory.Build("cancel");
            var errors = validator.Validate(appointmentDto);

            if (errors.Count == 0)
            {
                //book out equipment
                //create appointment
                //send email
                return Ok();
            }
            else
            {
                return BadRequest(errors);
            }
        }
    }
}
