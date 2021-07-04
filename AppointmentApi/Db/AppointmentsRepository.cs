using System.Linq;
using AppointmentApi.Dto;
using AppointmentApi.Db.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AppointmentApi.Db
{
    public class AppointmentsRepository : IAppointmentsRepository
    {
        private readonly AppointmentsContext _appointmentsContext;

        public AppointmentsRepository(AppointmentsContext appointmentsContext)
        {
            _appointmentsContext = appointmentsContext;
        }

        public async Task<Patient> GetPatientAsync(int patientId)
        {
            return await _appointmentsContext.Patients.Where(x => x.Id == patientId).SingleOrDefaultAsync();
        }

        public async Task<Appointment> GetAppointmentAsync(int patientId, DateTime dateTime)
        {
            var appointment = _appointmentsContext.Appointments.Where(x => x.PatientId == patientId
                                                                        && x.DateTime == dateTime
                                                                        && x.IsDeleted == false).SingleOrDefaultAsync();

            return await appointment;
        }

        public async Task<List<Appointment>> GetAppointmentsByDateAsync(DateTime date)
        {
            var appointments = _appointmentsContext.Appointments.Where(x => x.IsDeleted == false
                                                                         && x.DateTime.Date == date.Date);
                                                                     
            return await appointments.ToListAsync();
        }

        public async Task CreateAppointmentAsync(int patientId, DateTime dateTime, int equipmentId)
        {
            _appointmentsContext.Appointments.Add(new Appointment
            {
                PatientId = patientId,
                EquipmentId = equipmentId,
                DateTime = dateTime
            });
            await _appointmentsContext.SaveChangesAsync();
        }

        public async Task ChangeAppointmentAsync(int patientId, DateTime previousDateTime, DateTime newDateTime)
        {
            var appointment = await GetAppointmentAsync(patientId, previousDateTime);
            if(appointment != null)
            {
                appointment.DateTime = newDateTime;
                await _appointmentsContext.SaveChangesAsync();
            }
        }

        public async Task CancelAppointmentAsync(int patientId, DateTime dateTime)
        {
            var appointment = await GetAppointmentAsync(patientId, dateTime);
            if(appointment != null)
            {
                appointment.IsDeleted = true;
                await _appointmentsContext.SaveChangesAsync();
            }
        }
    }
}
