using AppointmentApi.Validation;
using System.Collections.Generic;

namespace AppointmentValidationSystem
{
    public abstract class AppointmentValidator<T> : IAppointmentValidator<T>
    {
        protected readonly IAppointmentParameters _appointmentParameters;
        protected List<ValidationError> ValidationErrors { get; }

        public AppointmentValidator(IAppointmentParameters appointmentParameters)
        {
            _appointmentParameters = appointmentParameters;
            ValidationErrors = new List<ValidationError>();
        }

        public abstract IList<ValidationError> Validate(T appointment);

        protected void AddValidationError(string message)
        {
            ValidationErrors.Add(new ValidationError(message));
        }
    }
}
