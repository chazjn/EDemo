using AppointmentApi.Db;
using AppointmentApi.Db.Models;
using AppointmentApi.Dto;
using AppointmentApi.Validation;
using AppointmentApi.Validation.ValidationErrors;
using EquipmentAvailabiltySystem;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests.AppointmentApi.Validation
{
    [TestFixture]
    public class CreateAppointmentValidatorTests : BaseValidatorTests
    {
        [TestCase("2021-01-10 13:00")]
        [TestCase("2021-01-11 16:00")]
        [TestCase("2021-01-24 11:00")]
        public void Validate_ValidAppointmentTime_ReturnsZeroErrors(string dateTime)
        {
            var appointmentValidator = GetCreateAppointmentValidator();

            var appointment = new AppointmentDto
            {
                PatientId = 1,
                DateTime = DateTime.Parse(dateTime)
            };

            var errors = appointmentValidator.Validate(appointment);

            Assert.That(errors.Count == 0);
        }

        [TestCase("2021-01-10 13:01")]
        [TestCase("2021-01-11 17:00")]
        [TestCase("2021-01-24 10:59")]
        public void Validate_InvalidAppointmentTime_ReturnError(string dateTime)
        {
            var appointmentValidator = GetCreateAppointmentValidator();

            var appointment = new AppointmentDto
            {
                PatientId = 1,
                DateTime = DateTime.Parse(dateTime)
            };

            var errors = appointmentValidator.Validate(appointment);

            Assert.That(errors.Count == 1 && errors.First() is AppointmentTimeInvalid);
        }

        [TestCase("2021-01-09 12:00")]
        [TestCase("2021-01-25 12:00")]
        public void Validate_InvalidAppointmentDate_ReturnError(string dateTime)
        {
            var appointmentValidator = GetCreateAppointmentValidator();

            var appointment = new AppointmentDto
            {
                PatientId = 1,
                DateTime = DateTime.Parse(dateTime)
            };

            var errors = appointmentValidator.Validate(appointment);

            Assert.That(errors.Count == 1 && errors.First() is AppointmentCreateOutOfRange);
        }
    }
}
