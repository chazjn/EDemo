namespace AppointmentApi.Validation
{
    public abstract class ValidationError
    {
        public string Message { get; }

        public ValidationError(string message)
        {
            Message = message;
        }
    }
}
