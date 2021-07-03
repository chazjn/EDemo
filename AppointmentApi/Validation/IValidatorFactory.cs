namespace AppointmentApi.Validation
{
    public interface IValidatorFactory
    {
        IAppointmentValidator Build(Validator validator);
    }
}
