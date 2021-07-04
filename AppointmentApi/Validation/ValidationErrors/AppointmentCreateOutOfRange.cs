using System;
namespace AppointmentApi.Validation.ValidationErrors
{
    public class AppointmentCreateOutOfRange : ValidationError
    {
        public AppointmentCreateOutOfRange(DateTime start, DateTime end) : base($"Appointments can only be made between {start} and {end}")
        {
        }
    }
}
