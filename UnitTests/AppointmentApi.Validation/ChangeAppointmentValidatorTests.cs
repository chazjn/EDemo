using AppointmentApi.Dto;
using AppointmentApi.Validation.ValidationErrors;
using NUnit.Framework;
using System;
using System.Linq;

namespace UnitTests.AppointmentApi.Validation
{
    [TestFixture]
    public class ChangeAppointmentValidatorTests : BaseValidatorTests
    {
        [TestCase("2021-01-13 12:00", "2021-01-12 12:00")]
        [TestCase("2021-01-15 12:00", "2021-01-14 12:00")]
        [TestCase("2021-01-24 11:00", "2021-01-23 11:00")]
        public void Validate_ChangeDateTimeInRange_ReturnsZeroErrors(string dateTime, string previousDateTime)
        {
            var appointmentValidator = GetChangeAppointmentValidator();

            var appointment = new ChangeAppointmentDto
            {
                PatientId = 1,
                DateTime = DateTime.Parse(dateTime),
                PreviousDateTime = DateTime.Parse(previousDateTime)
            };

            var errors = appointmentValidator.Validate(appointment);

            Assert.That(errors.Count == 0);
        }

        [TestCase("2021-01-11 12:00", "2021-01-10 12:00")]
        [TestCase("2021-01-13 12:00", "2021-01-11 12:00")]
        [TestCase("2021-01-13 11:00", "2021-01-12 11:00")]
        public void Validate_ChangeDateTimeOutOfRange_ReturnsError(string dateTime, string previousDateTime)
        {
            var appointmentValidator = GetChangeAppointmentValidator();

            var appointment = new ChangeAppointmentDto
            {
                PatientId = 1,
                DateTime = DateTime.Parse(dateTime),
                PreviousDateTime = DateTime.Parse(previousDateTime)
            };

            var errors = appointmentValidator.Validate(appointment);

            Assert.That(errors.Count == 1 && errors.First() is AppointmentAmendmentOutOfRange);
        }
    }
}
