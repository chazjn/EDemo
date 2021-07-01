using System;

namespace AppointmentApi.Dto
{
    public class AppointmentChangeDto : AppointmentDto
    {
        public DateTime NewDateTime { get; set; }
    }
}
