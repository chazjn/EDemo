using System;

namespace AppointmentApi.Validation
{
    public class StandardAppointmentParameters : IAppointmentParameters
    {
        public TimeSpan CanCreateBefore => TimeSpan.FromDays(14);
        public TimeSpan CanChangeBefore => TimeSpan.FromDays(2);
        public TimeSpan CanCancelBefore => TimeSpan.FromDays(3);
        public TimeSpan FirstAppointmentTimeOfDay => TimeSpan.FromHours(8);
        public TimeSpan LastAppointmentTimeOfDay => TimeSpan.FromHours(16);
        public TimeSpan AppointmentLength => TimeSpan.FromHours(1);
    }
}
