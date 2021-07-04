using AppointmentApi.Db;
using AppointmentApi.Dto;
using AppointmentApi.Validation;
using EquipmentAvailabiltySystem;
using System.Collections.Generic;

namespace AppointmentValidationSystem
{
    public abstract class AppointmentValidator<T> : IAppointmentValidator<T>
    {
        protected readonly IAppointmentParameters _appointmentParameters;
        protected readonly IAppointmentsRepository _appointmentsRepository;
        protected readonly IEquipmentAvailabilityService _equipmentAvailabiltyService;
        protected List<ValidationError> ValidationErrors { get; }

        public AppointmentValidator(IAppointmentParameters appointmentParameters, IAppointmentsRepository appointmentsRepository, IEquipmentAvailabilityService equipmentAvailabiltyService)
        {
            _appointmentParameters = appointmentParameters;
            _appointmentsRepository = appointmentsRepository;
            _equipmentAvailabiltyService = equipmentAvailabiltyService;
            ValidationErrors = new List<ValidationError>();
        }

        public abstract IList<ValidationError> Validate(T appointment);

        protected void AddValidationError(string message)
        {
            ValidationErrors.Add(new ValidationError(message));
        }
    }
}
