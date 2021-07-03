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

        public async Task<Appointment> GetAppointmentAsync(AppointmentDto appointmentDto)
        {
            var appointment = _appointmentsContext.Appointments.Where(x => x.PatientId == appointmentDto.PatientId
                                                                        && x.DateTime == appointmentDto.DateTime
                                                                        && x.IsDeleted == false).SingleOrDefaultAsync();

            return await appointment;
        }

        public async Task<List<Appointment>> GetAppointmentsByDateAsync(DateTime date)
        {
            var appointments = _appointmentsContext.Appointments.Where(x => x.IsDeleted == false
                                                                         && x.DateTime.Date == date.Date);
                                                                     
            return await appointments.ToListAsync();
        }

        public async Task CreateAppointmentAsync(AppointmentDto Dto, int equipmentId)
        {
            _appointmentsContext.Appointments.Add(new Appointment
            {
                PatientId = Dto.PatientId,
                EquipmentId = equipmentId,
                DateTime = Dto.DateTime
            });
            await _appointmentsContext.SaveChangesAsync();
        }

        public async Task ChangeAppointmentAsync(ChangeAppointmentDto appointmentChangeDto)
        {
            var appointment = await GetAppointmentAsync(appointmentChangeDto);
            if(appointment != null)
            {
                appointment.DateTime = appointmentChangeDto.NewDateTime;
                await _appointmentsContext.SaveChangesAsync();
            }
        }

        public async Task CancelAppointmentAsync(CancelAppointmentDto appointmentDto)
        {
            var appointment = await GetAppointmentAsync(appointmentDto);
            if(appointment != null)
            {
                appointment.IsDeleted = true;
                await _appointmentsContext.SaveChangesAsync();
            }
        }
    }
}
