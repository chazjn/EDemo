using System;

namespace AppointmentApi.Dto
{
    public interface IAppointmentDto
    {
        int PatientId { get; set; }
        DateTime DateTime { get; set; }
    }
}
