using AppointmentApi.Dto;
using System;

namespace AppointmentApi.Validation
{
    public class ValidatorFactory : IValidatorFactory
    {
        private readonly IAppointmentParameters _appointmentParameters;

        public ValidatorFactory(IAppointmentParameters appointmentParameters)
        {
            _appointmentParameters = appointmentParameters;
        }

        public IAppointmentValidator<T> Build<T>(T appointment)
        {
            return (typeof(T)) switch
            {
                var type when type == typeof(AppointmentDto) => new CreateAppointmentValidator(_appointmentParameters) as IAppointmentValidator<T>,
                var type when type == typeof(ChangeAppointmentDto) => new ChangeAppointmentValidator(_appointmentParameters) as IAppointmentValidator<T>,
                var type when type == typeof(CancelAppointmentDto) => new CancelAppointmentValidator(_appointmentParameters) as IAppointmentValidator<T>,
                _ => throw new ArgumentException($"No validator found for type '{typeof(T)}'"),
            };
        }
    }
}