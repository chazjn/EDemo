using AppointmentApi.Dto;
using AppointmentApi.Validation.ValidationErrors;
using NUnit.Framework;
using System;
using System.Linq;

namespace UnitTests.AppointmentApi.Validation
{
    [TestFixture]
    public class CancelAppointmentValidatorTests : BaseValidatorTests
    {
        [TestCase("2021-01-13 12:00")]
        [TestCase("2021-01-14 12:00")]
        [TestCase("2021-01-24 12:00")]
        public void Validate_CancellationDateTimeInRange_ReturnsZeroErrors(string dateTime)
        {
            var appointmentValidator = GetCancelAppointmentValidator();

            var appointment = new CancelAppointmentDto
            {
                PatientId = 1,
                DateTime = DateTime.Parse(dateTime)
            };

            var errors = appointmentValidator.Validate(appointment);

            Assert.That(errors.Count == 0);
        }

        [TestCase("2021-01-10 12:00")]
        [TestCase("2021-01-11 12:00")]
        [TestCase("2021-01-13 11:00")]
        public void Validate_CancellationDateTimeOutOfRange_ReturnsError(string dateTime)
        {
            var appointmentValidator = GetCancelAppointmentValidator();

            var appointment = new CancelAppointmentDto
            {
                PatientId = 1,
                DateTime = DateTime.Parse(dateTime)
            };

            var errors = appointmentValidator.Validate(appointment);

            Assert.That(errors.Count == 1 && errors.First() is AppointmentAmendmentOutOfRange);
        }
    }
}
