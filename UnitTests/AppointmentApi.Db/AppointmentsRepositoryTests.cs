using AppointmentApi.Db;
using AppointmentApi.Db.Models;
using AppointmentApi.Dto;
using AppointmentApi.Validation.ValidationErrors;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.AppointmentApi.Db
{
    [TestFixture]
    public class AppointmentsRepositoryTests
    {
        AppointmentsContext _appointmentsContext;
        IAppointmentsRepository _appointmentsRepository;

        [SetUp]
        public void SetUp()
        {
            _appointmentsContext = FakeServices.AppointmentsContext;
            _appointmentsRepository = new AppointmentsRepository(_appointmentsContext);
        }

        [TearDown]
        public void TearDown()
        {
            _appointmentsContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task TryCreateAppointmentAsync_PatientDoesNotExist_ReturnsValidationError()
        {
            var validationErrors = await _appointmentsRepository.TryCreateAppointmentAsync(1, DateTime.Now, 1);

            Assert.That(validationErrors.Single() is PatientDoesNotExist);
        }

        [Test]
        public async Task TryCreateAppointment_AppointmentAlreadyExists_ReturnsValidationError()
        {
            var patientId = 1;
            var appointmentDateTime = DateTime.Parse("2021-01-01 10:00");

            _appointmentsContext.Add(new Patient());
            _appointmentsContext.Add(new Appointment
            {
                PatientId = patientId,
                DateTime = appointmentDateTime
            });
            _appointmentsContext.SaveChanges();

            var validationErrors = await _appointmentsRepository.TryCreateAppointmentAsync(patientId, appointmentDateTime, 1);

            Assert.That(validationErrors.Single() is AppointmentAlreadyExists);
        }

        [Test]
        public async Task TryChangeAppointment_PatientDoesNotExist_ReturnsValidationError()
        {
            var validationErrors = await _appointmentsRepository.TryChangeAppointmentAsync(1, DateTime.Now, DateTime.Now, 1);

            Assert.That(validationErrors.Where(v => v.GetType() == typeof(PatientDoesNotExist)).Count() == 1);
        }
    }
}
