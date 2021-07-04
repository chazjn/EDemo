using AppointmentApi.Db;
using AppointmentApi.Dto;
using EquipmentAvailabiltySystem;
using System;

namespace AppointmentApi.Validation
{
    public class ValidatorFactory : IValidatorFactory
    {
        private readonly IAppointmentParameters _appointmentParameters;
        private readonly IAppointmentsRepository _appointmentsRepository;
        private readonly IEquipmentAvailabilityService _equipmentAvailabiltyService;

        public ValidatorFactory(IAppointmentParameters appointmentParameters, IAppointmentsRepository appointmentsRepository, IEquipmentAvailabilityService equipmentAvailabiltyService)
        {
            _appointmentParameters = appointmentParameters;
            _appointmentsRepository = appointmentsRepository;
            _equipmentAvailabiltyService = equipmentAvailabiltyService;
        }

        public IAppointmentValidator<T> Build<T>(T appointment)
        {
            return (typeof(T)) switch
            {
                var type when type == typeof(AppointmentDto) => new CreateAppointmentValidator(_appointmentParameters, _appointmentsRepository, _equipmentAvailabiltyService) as IAppointmentValidator<T>,
                var type when type == typeof(ChangeAppointmentDto) => new ChangeAppointmentValidator(_appointmentParameters, _appointmentsRepository, _equipmentAvailabiltyService) as IAppointmentValidator<T>,
                var type when type == typeof(CancelAppointmentDto) => new CancelAppointmentValidator(_appointmentParameters, _appointmentsRepository, _equipmentAvailabiltyService) as IAppointmentValidator<T>,
                _ => throw new ArgumentException($"No validator found for type '{typeof(T)}'"),
            };
        }
    }
}