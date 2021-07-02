﻿using AppointmentApi.Db;
using AppointmentApi.Dto;
using AppointmentValidationSystem;
using EquipmentAvailabilty;
using System;
using System.Collections.Generic;

namespace AppointmentApi.Validation
{
    public class CreateAppointmentValidator: AppointmentValidator
    {
        public CreateAppointmentValidator(IAppointmentParameters appointmentParameters, IAppointmentsRepository appointmentsRepository, IEquipmentAvailabilityService equipmentAvailabiltyService) : base(appointmentParameters, appointmentsRepository, equipmentAvailabiltyService)
        {
        }

        public override IList<ValidationError> Validate<AppointmentDto>(AppointmentDto appointment)
        {
            //TODO: check datetime is on the hour
            //check datetime start time
            //check days befor
            //check user exists
            //check equipment is available

            if(appointment.DateTime.TimeOfDay < _appointmentParameters.FirstAppointmentTimeOfDay
            || appointment.DateTime.TimeOfDay > _appointmentParameters.LastAppointmentTimeOfDay)
            {
                AddValidationError($"Appointment must be between {_appointmentParameters.FirstAppointmentTimeOfDay} and {_appointmentParameters.LastAppointmentTimeOfDay}");
            }

            var cutoffDateTime = (DateTime.Now + _appointmentParameters.CanCreateBefore) - _appointmentParameters.AppointmentLength;
            if (appointment.DateTime > cutoffDateTime)
            {
                AddValidationError($"Appointment be made after {cutoffDateTime}");
            }

            if (_appointmentsRepository.GetPatient(appointment.PatientId) == null)
            {
                AddValidationError($"Patient Id {appointment.PatientId} does not exist");
            }

            if(_equipmentAvailabiltyService.GetAvailability(appointment.DateTime).Count == 0)
            {
                AddValidationError($"No equipment available on {appointment.DateTime}");
            }

            return ValidationErrors;
        }
    }
}
