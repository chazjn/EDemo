using AppointmentApi.Dto;
using AppointmentApi.Db.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppointmentApi.Validation;

namespace AppointmentApi.Db
{
    public interface IAppointmentsRepository
    {
        Task<Patient> GetPatientAsync(int patientId);

        Task<Appointment> GetAppointmentAsync(int patientId, DateTime dateTime);

        Task<List<Appointment>> GetAppointmentsByDateAsync(DateTime date);

        Task<IList<ValidationError>>  TryCreateAppointmentAsync(int patientId, DateTime dateTime, int equipmentId);

        Task<IList<ValidationError>> TryChangeAppointmentAsync(int patientId, DateTime previousDateTime, DateTime newDateTime);

        Task<IList<ValidationError>> TryCancelAppointmentAsync(int patientId, DateTime dateTime);
    }
}
