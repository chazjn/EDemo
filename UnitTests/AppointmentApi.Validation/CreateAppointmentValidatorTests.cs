using AppointmentApi.Db;
using AppointmentApi.Db.Models;
using AppointmentApi.Dto;
using AppointmentApi.Validation;
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
        [Test]
        public void Validate_WhenCalledWithValidAppointment_ReturnsZeroErrors()
        {
            //add patient to database
            SeedDatabase(1);

            //ensure equipment is available
            EquipmentAvailabilityService = FakeServices.EquipmentAvailabilityService_WithAvailabilityOn(ValidAppointmentDateTime);

            var appointmentValidator = GetValidator();

            var appointment = new AppointmentDto
            {
                PatientId = 1,
                DateTime = ValidAppointmentDateTime
            };

            var errors = appointmentValidator.Validate(appointment);

            Assert.That(errors.Count == 0);
        }

        [Test]
        public void Validate_WhenCalledWithInvalidAppointmentDate_ReturnError()
        {
            //add patient to database
            SeedDatabase(1);

            //ensure equipment is available
            EquipmentAvailabilityService = FakeServices.EquipmentAvailabilityService_WithAvailabilityOn(InvalidAppointmentDateTime);

            var appointmentValidator = GetValidator();

            var appointment = new AppointmentDto
            {
                PatientId = 1,
                DateTime = InvalidAppointmentDateTime
            };

            var errors = appointmentValidator.Validate(appointment);

            Assert.That(errors.Count == 1 && errors.First().Message.Contains("Appointment must be between"));
        }

        [Test]
        public void Validate_WhenCalledWithInvalidPatient_ReturnError()
        {
            //ensure equipment is available
            EquipmentAvailabilityService = FakeServices.EquipmentAvailabilityService_WithAvailabilityOn(ValidAppointmentDateTime);

            var appointmentValidator = GetValidator();

            var appointment = new AppointmentDto
            {
                PatientId = 1,
                DateTime = ValidAppointmentDateTime
            };

            var errors = appointmentValidator.Validate(appointment);

            Assert.That(errors.Count == 1 && errors.First().Message.Contains("Patient Id 1 does not exist"));
        }
    }
}
