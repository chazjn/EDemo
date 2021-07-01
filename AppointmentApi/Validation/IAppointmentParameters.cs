using System;

namespace AppointmentApi.Validation
{
    public interface IAppointmentParameters
    {
        TimeSpan CanCreateBefore { get; }
        TimeSpan CanChangeBefore { get; }
        TimeSpan CanCancelBefore { get; }
        TimeSpan FirstAppointmentTimeOfDay { get; }
        TimeSpan LastAppointmentTimeOfDay { get; }
        TimeSpan AppointmentLength { get; }
    }
}
