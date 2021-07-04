using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApi.Validation.ValidationErrors
{
    public class AppointmentDoesNotExist : ValidationError
    {
        public AppointmentDoesNotExist(int patientId, DateTime dateTime) : base($"Appointment for patient {patientId} on {dateTime} does not exist")
        {
        }
    }
}
