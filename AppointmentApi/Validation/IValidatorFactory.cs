namespace AppointmentApi.Validation
{
    public interface IValidatorFactory
    {
        IAppointmentValidator<T> Build<T>(T appointment);
    }
}
