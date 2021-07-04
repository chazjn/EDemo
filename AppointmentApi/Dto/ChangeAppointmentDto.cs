using System;

namespace AppointmentApi.Dto
{
    public class ChangeAppointmentDto : AppointmentDto
    {
        public DateTime PreviousDateTime { get; set; }
    }
}
