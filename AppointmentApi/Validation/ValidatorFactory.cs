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

        public IAppointmentValidator Build(string type)
        {
            switch (type)
            {
                case "create":
                    return new CreateAppointmentValidator(_appointmentParameters, _appointmentsRepository, _equipmentAvailabiltyService);

                case "change":
                    return new ChangeAppointmentValidator(_appointmentParameters, _appointmentsRepository, _equipmentAvailabiltyService);

                case "cancel":
                    return new CancelAppointmentValidator(_appointmentParameters, _appointmentsRepository, _equipmentAvailabiltyService);

                default:
                    throw new ArgumentException($"No validator found for type {type}");
            }
        }
    }
}
