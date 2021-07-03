using System;

namespace AppointmentApi.Dto
{
    public class CancelAppointmentDto : IAppointmentDto
    {
        public int PatientId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
