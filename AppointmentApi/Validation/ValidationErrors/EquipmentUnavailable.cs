using System;

namespace AppointmentApi.Validation.ValidationErrors
{
    public class EquipmentUnavailable : ValidationError
    {
        public EquipmentUnavailable(DateTime dateTime) : base($"No equipment available on {dateTime}")
        {
        }
    }
}
