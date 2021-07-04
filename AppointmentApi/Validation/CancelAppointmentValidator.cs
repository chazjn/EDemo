using AppointmentApi.Dto;
using AppointmentValidationSystem;
using System;
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
            var cutoff = DateTime.Now + _appointmentParameters.CanCancelBefore;
            if(appointment.DateTime < cutoff)
            {
                AddValidationError($"Cannot cancel appointments that are booked before {cutoff}");
            }

            return ValidationErrors;
        }
    }
}
