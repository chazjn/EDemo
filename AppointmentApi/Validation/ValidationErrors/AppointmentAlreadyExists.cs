using System;

namespace AppointmentApi.Validation.ValidationErrors
{
    public class AppointmentAlreadyExists : ValidationError
    {
        public AppointmentAlreadyExists(int patientId, DateTime dateTime) : base($"Patient {patientId} already has appointment on {dateTime}")
        {
            
        }
    }
}
