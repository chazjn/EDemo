using AppointmentApi.Db;
using AppointmentApi.Dto;
using AppointmentValidationSystem;
using EquipmentAvailabilty;
using System;
using System.Collections.Generic;

namespace AppointmentApi.Validation
{
    public class CancelAppointmentValidator : AppointmentValidator<AppointmentDto>
    {
        public CancelAppointmentValidator(IAppointmentParameters appointmentParameters, IAppointmentsRepository appointmentsRepository, IEquipmentAvailabilityService equipmentAvailabiltyService) : base(appointmentParameters, appointmentsRepository, equipmentAvailabiltyService)
        {
        }

        public override IList<ValidationError> Validate(AppointmentDto appointment)
        {
            //patient exists
            //appointment exists
            //3 days before
            
            if (_appointmentsRepository.GetPatient(appointment.PatientId) == null)
            {
                AddValidationError($"Appointment on {appointment.DateTime} does not exist");
            }

            if (_appointmentsRepository.GetAppointment(appointment) == null)
            {
                AddValidationError($"Patient Id {appointment.PatientId} does not exist");
            }

            var cutoff = DateTime.Now + _appointmentParameters.CanCancelBefore;
            if(appointment.DateTime < cutoff)
            {
                AddValidationError($"Cannot cancel appointments made before {cutoff}");
            }

            return ValidationErrors;
        }
    }
}
