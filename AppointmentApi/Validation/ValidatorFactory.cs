using AppointmentApi.Db;
using EquipmentAvailabilty;
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

        public IAppointmentValidator Build(Validator validator)
        {
            return validator switch
            {
                Validator.Create => new CreateAppointmentValidator(_appointmentParameters, _appointmentsRepository, _equipmentAvailabiltyService),
                Validator.Change => new ChangeAppointmentValidator(_appointmentParameters, _appointmentsRepository, _equipmentAvailabiltyService),
                Validator.Cancel => new CancelAppointmentValidator(_appointmentParameters, _appointmentsRepository, _equipmentAvailabiltyService),
                _ => throw new ArgumentException($"No validator found for type '{validator}'"),
            };
        }
    }
}
