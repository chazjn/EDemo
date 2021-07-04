using AppointmentApi.Validation;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace AppointmentValidationSystem
{
    public abstract class AppointmentValidator<T> : IAppointmentValidator<T>
    {
        protected readonly IAppointmentParameters _appointmentParameters;
        protected List<ValidationError> ValidationErrors { get; }
        public DateTime Now { get; internal set; }

        public AppointmentValidator(IAppointmentParameters appointmentParameters)
        {
            _appointmentParameters = appointmentParameters;
            ValidationErrors = new List<ValidationError>();
            Now = DateTime.Now;
        }

        public abstract IList<ValidationError> Validate(T appointment);

        protected void AddValidationError(ValidationError error)
        {
            ValidationErrors.Add(error);
        }
    }
}
