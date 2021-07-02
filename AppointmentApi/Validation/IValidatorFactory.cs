namespace AppointmentApi.Validation
{
    public interface IValidatorFactory
    {
        IAppointmentValidator Build(string type);
    }
}
