using System;

namespace AppointmentApi.Dto
{
    public class ChangeAppointmentDto : AppointmentDto
    {
        public DateTime NewDateTime { get; set; }
    }
}
