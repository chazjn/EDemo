using System;
namespace AppointmentApi.Validation.ValidationErrors
{
    public class AppointmentAmendmentOutOfRange : ValidationError
    {
        public AppointmentAmendmentOutOfRange(DateTime cutoffDateTime) : base($"Cut off time for amending this appointment was {cutoffDateTime}")
        {
        }
    }
}
