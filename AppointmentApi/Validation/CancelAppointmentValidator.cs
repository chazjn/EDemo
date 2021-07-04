using AppointmentApi.Dto;
using AppointmentApi.Validation.ValidationErrors;
using AppointmentValidationSystem;
using System.Collections.Generic;

namespace AppointmentApi.Validation
{
    public class CancelAppointmentValidator : AppointmentValidator<CancelAppointmentDto>
    {
        public CancelAppointmentValidator(IAppointmentParameters appointmentParameters) : base(appointmentParameters)
        {
        }

        public override IList<ValidationError> Validate(CancelAppointmentDto appointment)
        {
            var cutoff = appointment.DateTime - _appointmentParameters.CanCancelBefore;
            if (Now > cutoff)
            {
                AddValidationError(new AppointmentAmendmentOutOfRange(cutoff));
            }

            return ValidationErrors;
        }
    }
}
