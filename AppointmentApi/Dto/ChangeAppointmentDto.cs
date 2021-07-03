using System;

namespace AppointmentApi.Dto
{
    public class ChangeAppointmentDto : IAppointmentDto
    {
        public int PatientId { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime NewDateTime { get; set; }
    }
}
