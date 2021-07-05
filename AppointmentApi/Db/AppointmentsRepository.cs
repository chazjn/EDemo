using System.Linq;
using AppointmentApi.Dto;
using AppointmentApi.Db.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AppointmentApi.Validation;
using AppointmentApi.Validation.ValidationErrors;

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

        public async Task<IList<ValidationError>> TryCreateAppointmentAsync(int patientId, DateTime dateTime, int equipmentId)
        {
            var errors = new List<ValidationError>();

            var patient = await GetPatientAsync(patientId);
            if (patient == null)
                errors.Add(new PatientDoesNotExist(patientId));

            var appointment = await GetAppointmentAsync(patientId, dateTime);
            if (appointment != null)
                errors.Add(new AppointmentAlreadyExists(patientId, dateTime));

            if(patient != null && appointment == null)
            {
                _appointmentsContext.Appointments.Add(new Appointment
                {
                    PatientId = patientId,
                    EquipmentId = equipmentId,
                    DateTime = dateTime
                });
                await _appointmentsContext.SaveChangesAsync();
            }

            return errors;
        }

        public async Task<IList<ValidationError>> TryChangeAppointmentAsync(int patientId, DateTime previousDateTime, DateTime newDateTime, int equipmentId)
        {
            var errors = new List<ValidationError>();

            var patient = await GetPatientAsync(patientId);
            if (patient == null)
                errors.Add(new PatientDoesNotExist(patientId));

            var previousAppointment = await GetAppointmentAsync(patientId, previousDateTime);
            if (previousAppointment == null)
                errors.Add(new AppointmentDoesNotExist(patientId, previousDateTime));

            var newAppointment = await GetAppointmentAsync(patientId, newDateTime);
            if (newAppointment != null)
                errors.Add(new AppointmentAlreadyExists(patientId, newDateTime));
            
            if(newAppointment == null && previousAppointment != null)
            {
                previousAppointment.IsDeleted = true;
                _appointmentsContext.Appointments.Add(new Appointment
                {
                    PatientId = patientId,
                    EquipmentId = equipmentId,
                    DateTime = newDateTime
                });
                await _appointmentsContext.SaveChangesAsync();
            }

            return errors;
        }

        public async Task<IList<ValidationError>> TryCancelAppointmentAsync(int patientId, DateTime dateTime)
        {
            var errors = new List<ValidationError>();

            var patient = await GetPatientAsync(patientId);
            if (patient == null)
                errors.Add(new PatientDoesNotExist(patientId));
            
            var appointment = await GetAppointmentAsync(patientId, dateTime);
            if (appointment == null)
            {
                errors.Add(new AppointmentDoesNotExist(patientId, dateTime));
            }
            else
            {
                appointment.IsDeleted = true;
                await _appointmentsContext.SaveChangesAsync();
            }
                
            return errors;
        }
    }
}
