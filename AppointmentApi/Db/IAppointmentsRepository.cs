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

        Task<Appointment> GetAppointmentAsync(int patientId, DateTime dateTime);

        Task<List<Appointment>> GetAppointmentsByDateAsync(DateTime date);

        Task CreateAppointmentAsync(int patientId, DateTime dateTime, int equipmentId);

        Task ChangeAppointmentAsync(int patientId, DateTime previousDateTime, DateTime newDateTime);

        Task CancelAppointmentAsync(int patientId, DateTime dateTime);
    }
}
