namespace AppointmentApi.Validation.ValidationErrors
{
    public class PatientDoesNotExist : ValidationError
    {
        public PatientDoesNotExist(int patientId) : base($"Patient {patientId} does not exist")
        {
        }
    }
}
