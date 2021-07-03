using System;

namespace AppointmentApi.Dto
{
    public class CreateAppointmentDto : IAppointmentDto
    {
        public int PatientId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
