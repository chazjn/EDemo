using AppointmentApi.Dto;
using AppointmentApi.Db.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentApi.Db
{
    public interface IAppointmentsRepository
    {
        Task<Patient> GetPatientAsync(int patientId);

        Task<Appointment> GetAppointmentAsync(AppointmentDto appointment);

        Task<List<Appointment>> GetAppointmentsByDateAsync(DateTime date);

        Task CreateAppointmentAsync(AppointmentDto Dto, int equipmentId);

        Task ChangeAppointmentAsync(AppointmentChangeDto appointmentChangeDto);

        Task CancelAppointmentAsync(AppointmentDto appointmentDto);
    }
}
