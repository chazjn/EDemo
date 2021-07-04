using AppointmentApi.Db;
using AppointmentApi.Dto;
using AppointmentValidationSystem;
using EquipmentAvailabiltySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApi.Validation
{
    public class ChangeAppointmentValidator : AppointmentValidator<ChangeAppointmentDto>
    {
        public ChangeAppointmentValidator(IAppointmentParameters appointmentParameters, IAppointmentsRepository appointmentsRepository, IEquipmentAvailabilityService equipmentAvailabiltyService) : base(appointmentParameters, appointmentsRepository, equipmentAvailabiltyService)
        {
        }

        public override IList<ValidationError> Validate(ChangeAppointmentDto appointment)
        {
            //Same as create
            //2 days before

            var createAppointmentValidator = new CreateAppointmentValidator(_appointmentParameters, _appointmentsRepository, _equipmentAvailabiltyService);
            var createAppointmentErrors = createAppointmentValidator.Validate(appointment);
            ValidationErrors.AddRange(createAppointmentErrors);

            if (appointment.NewDateTime.Minute != 0)
            {
                AddValidationError($"Appointment must be made on the hour");
            }

            var cutoff = DateTime.Now + _appointmentParameters.CanChangeBefore;
            if (appointment.NewDateTime < cutoff)
            {
                AddValidationError($"Cannot change appointments that are booked before {cutoff}");
            }

            return ValidationErrors;
        }
    }
}
