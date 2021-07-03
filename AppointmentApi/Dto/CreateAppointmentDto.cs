using System;

namespace AppointmentApi.Dto
{
    public class CreateAppointmentDto
    {
        public int PatientId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
