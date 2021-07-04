using System;

namespace AppointmentApi.Validation.ValidationErrors
{
    public class AppointmentTimeInvalid : ValidationError
    {
        public AppointmentTimeInvalid(TimeSpan time) : base($"Appointment time {time} is not valid")
        {
        }
    }
}
