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

        Task<Appointment> GetAppointmentAsync(IAppointmentDto appointment);

        Task<List<Appointment>> GetAppointmentsByDateAsync(DateTime date);

        Task CreateAppointmentAsync(IAppointmentDto Dto, int equipmentId);

        Task ChangeAppointmentAsync(ChangeAppointmentDto appointmentDto);

        Task CancelAppointmentAsync(IAppointmentDto appointmentDto);
    }
}
