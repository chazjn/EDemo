using AppointmentApi.Dto;
using AppointmentApi.Validation.ValidationErrors;
using AppointmentValidationSystem;
using System.Collections.Generic;

namespace AppointmentApi.Validation
{
    public class ChangeAppointmentValidator : AppointmentValidator<ChangeAppointmentDto>
    {
        public ChangeAppointmentValidator(IAppointmentParameters appointmentParameters) : base(appointmentParameters)
        {
        }

        public override IList<ValidationError> Validate(ChangeAppointmentDto appointment)
        {
            var createAppointmentValidator = new CreateAppointmentValidator(_appointmentParameters)
            {
                Now = Now
            };
            var createAppointmentErrors = createAppointmentValidator.Validate(appointment);
            ValidationErrors.AddRange(createAppointmentErrors);

            var cutoff = appointment.PreviousDateTime - _appointmentParameters.CanChangeBefore;
            if (Now > cutoff)
            {
                AddValidationError(new AppointmentAmendmentOutOfRange(cutoff));
            }

            return ValidationErrors;
        }
    }
}
