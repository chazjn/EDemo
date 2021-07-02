using AppointmentApi.Db;
using AppointmentApi.Dto;
using AppointmentApi.Validation;
using EquipmentAvailabilty;
using System.Collections.Generic;

namespace AppointmentValidationSystem
{
    public abstract class AppointmentValidator<T> where T : AppointmentDto
    {
        protected readonly IAppointmentParameters _appointmentParameters;
        protected readonly IAppointmentsRepository _appointmentsRepository;
        protected readonly IEquipmentAvailabilityService _equipmentAvailabiltyService;
        protected IList<ValidationError> ValidationErrors { get; }

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
