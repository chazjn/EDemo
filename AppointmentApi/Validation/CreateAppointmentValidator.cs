using AppointmentApi.Dto;
using AppointmentApi.Validation.ValidationErrors;
using AppointmentValidationSystem;
using System.Collections.Generic;

namespace AppointmentApi.Validation
{
    public class CreateAppointmentValidator: AppointmentValidator<AppointmentDto>
    {
        public CreateAppointmentValidator(IAppointmentParameters appointmentParameters) : base(appointmentParameters)
        {
        }

        public override IList<ValidationError> Validate(AppointmentDto appointment)
        {
            //ccheck datetime is on the hour and between the allowed times of data
            if (appointment.DateTime.Minute != 0
             || appointment.DateTime.TimeOfDay < _appointmentParameters.FirstAppointmentTimeOfDay
             || appointment.DateTime.TimeOfDay > _appointmentParameters.LastAppointmentTimeOfDay)
            {
                AddValidationError(new AppointmentTimeInvalid(appointment.DateTime.TimeOfDay));
            }

            //check datetime is within the valid date range
            var cutoffDateTime = (Now + _appointmentParameters.CanCreateBefore) - _appointmentParameters.AppointmentLength;
            if (appointment.DateTime < Now || appointment.DateTime > cutoffDateTime)
            {
                AddValidationError(new AppointmentCreateOutOfRange(Now, cutoffDateTime));
            }

            return ValidationErrors;
        }
    }
}