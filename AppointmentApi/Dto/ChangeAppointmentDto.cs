using System;

namespace AppointmentApi.Dto
{
    public class ChangeAppointmentDto : CreateAppointmentDto
    {
        public DateTime NewDateTime { get; set; }
    }
}
