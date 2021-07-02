using AppointmentApi.Dto;
using System.Collections.Generic;

namespace AppointmentApi.Validation
{
    public interface IAppointmentValidator
    {
        IList<ValidationError> Validate<T>(T appointment) where T : AppointmentDto;
    }
}
