using System.Linq;
using AppointmentApi.Dto;
using AppointmentApi.Db.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApi.Db
{
    public class AppointmentsRepository : IAppointmentsRepository
    {
        private readonly AppointmentsContext _appointmentsContext;

        public AppointmentsRepository(AppointmentsContext appointmentsContext)
        {
            _appointmentsContext = appointmentsContext;
        }

        public Patient GetPatient(int patientId)
        {
            return _appointmentsContext.Patients.Where(x => x.Id == patientId).SingleOrDefault();
        }

        public Appointment GetAppointment(AppointmentDto appointment)
        {
            return _appointmentsContext.Appointments.Where(x => x.PatientId == appointment.PatientId
                                                             && x.DateTime == appointment.DateTime
                                                             && x.IsDeleted == false).SingleOrDefault();
        }

        public IList<Appointment> GetAppointmentsByDate(DateTime date)
        {
            return _appointmentsContext.Appointments.Where(x => x.IsDeleted == false
                                                             && x.DateTime.Date == date.Date).ToList();
        }

        public void CreateAppointment(AppointmentDto Dto, int equipmentId)
        {
            _appointmentsContext.Appointments.Add(new Appointment
            {
                PatientId = Dto.PatientId,
                EquipmentId = equipmentId,
                DateTime = Dto.DateTime
            });
            _appointmentsContext.SaveChanges();
        }

        public void ChangeAppointment(AppointmentChangeDto appointmentChangeDto)
        {
            var appointment = GetAppointment(appointmentChangeDto);
            if(appointment != null)
            {
                appointment.DateTime = appointmentChangeDto.NewDateTime;
                _appointmentsContext.SaveChanges();
            }
        }

        public void CancelAppointment(AppointmentDto appointmentDto)
        {
            var appointment = GetAppointment(appointmentDto);
            if(appointment != null)
            {
                appointment.IsDeleted = true;
                _appointmentsContext.SaveChanges();
            }
        }
    }
}
