using AppointmentApi.Dto;
using AppointmentValidationSystem;
using System;
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
            //check datetime is on the hour
            //check datetime start time
            //check days before
            if(appointment.DateTime.Minute != 0)
            {
                AddValidationError($"Appointment must be made on the hour");
            }

            if(appointment.DateTime.TimeOfDay < _appointmentParameters.FirstAppointmentTimeOfDay
            || appointment.DateTime.TimeOfDay > _appointmentParameters.LastAppointmentTimeOfDay)
            {
                AddValidationError($"Appointment must be between {_appointmentParameters.FirstAppointmentTimeOfDay} and {_appointmentParameters.LastAppointmentTimeOfDay}");
            }

            var cutoffDateTime = (DateTime.Now + _appointmentParameters.CanCreateBefore) - _appointmentParameters.AppointmentLength;
            if (appointment.DateTime > cutoffDateTime)
            {
                AddValidationError($"Appointment cannot be made after {cutoffDateTime}");
            }

            return ValidationErrors;
        }
    }
}
