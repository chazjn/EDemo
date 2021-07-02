using AppointmentApi.Db;
using AppointmentApi.Dto;
using AppointmentValidationSystem;
using EquipmentAvailabilty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApi.Validation
{
    public class ChangeAppointmentValidator : AppointmentValidator
    {
        public ChangeAppointmentValidator(IAppointmentParameters appointmentParameters, IAppointmentsRepository appointmentsRepository, IEquipmentAvailabilityService equipmentAvailabiltyService) : base(appointmentParameters, appointmentsRepository, equipmentAvailabiltyService)
        {
        }

        public override IList<ValidationError> Validate<AppointmentChangeDto>(AppointmentChangeDto appointment)
        {
            //Same as create
            //2 days before

            var createAppointmentValidator = new CreateAppointmentValidator(_appointmentParameters, _appointmentsRepository, _equipmentAvailabiltyService);
            var createAppointmentErrors = createAppointmentValidator.Validate(appointment);
            ValidationErrors.AddRange(createAppointmentErrors);

            var cutoff = DateTime.Now + _appointmentParameters.CanChangeBefore;
            if (appointment.DateTime < cutoff)
            {
                AddValidationError($"Cannot change appointments that are booked before {cutoff}");
            }

            return ValidationErrors;
        }
    }
}
