using AppointmentApi.Dto;
using AppointmentApi.Db.Models;

namespace AppointmentApi.Db
{
    public interface IAppointmentsRepository
    {
        Patient GetPatient(int patientId);

        Appointment GetAppointment(AppointmentDto appointment);

        void CreateAppointment(AppointmentDto Dto, int equipmentId);

        void ChangeAppointment(AppointmentChangeDto appointmentChangeDto);

        void CancelAppointment(AppointmentDto appointmentDto);
    }
}
