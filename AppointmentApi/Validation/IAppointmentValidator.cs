using AppointmentApi.Dto;
using System.Collections.Generic;

namespace AppointmentApi.Validation
{
    public interface IAppointmentValidator<T>
    {
        IList<ValidationError> Validate(T appointment);
    }
}
