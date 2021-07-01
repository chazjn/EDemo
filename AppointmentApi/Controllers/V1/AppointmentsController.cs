using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EquipmentAvailabilty;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AppointmentApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly ILogger<AppointmentsController> _logger;
        private readonly IEquipmentAvailabilityService _equipmentAvailabilityService;

        public AppointmentsController(ILogger<AppointmentsController> logger, IEquipmentAvailabilityService equipmentAvailabilityService)
        {
            _logger = logger;
            _equipmentAvailabilityService = equipmentAvailabilityService;
        }

        [HttpGet]
        public IActionResult List()
        {
            return Ok("List");
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create()
        {
            return Ok("Create");
        }

        [HttpPost]
        [Route("Change")]
        public IActionResult Change()
        {
            return Ok("Change");
        }

        [HttpPost]
        [Route("Cancel")]
        public IActionResult Cancel()
        {
            return Ok("Cancel");
        }
    }
}
