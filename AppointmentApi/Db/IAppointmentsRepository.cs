using AppointmentApi.Dto;
using AppointmentApi.Db.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentApi.Db
{
    public interface IAppointmentsRepository
    {
        Patient GetPatient(int patientId);

        Appointment GetAppointment(AppointmentDto appointment);

        Task<List<AppointmentDto>> GetAppointmentsByDateAsync(DateTime date);

        void CreateAppointment(AppointmentDto Dto, int equipmentId);

        void ChangeAppointment(AppointmentChangeDto appointmentChangeDto);

        void CancelAppointment(AppointmentDto appointmentDto);
    }
}
