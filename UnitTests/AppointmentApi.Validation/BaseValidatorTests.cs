using AppointmentApi.Validation;
using System;

namespace UnitTests.AppointmentApi.Validation
{
    public abstract class BaseValidatorTests
    {
        public DateTime TestDateTime { get; }
        
        public BaseValidatorTests()
        {
            TestDateTime = DateTime.Parse("2021-01-10 12:00");
        }
        
        public CreateAppointmentValidator GetCreateAppointmentValidator()
        {
            return new CreateAppointmentValidator(new StandardAppointmentParameters())
            {
                Now = TestDateTime
            };
        }

        public ChangeAppointmentValidator GetChangeAppointmentValidator()
        {
            return new ChangeAppointmentValidator(new StandardAppointmentParameters())
            {
                Now = TestDateTime
            };
        }

        public CancelAppointmentValidator GetCancelAppointmentValidator()
        {
            return new CancelAppointmentValidator(new StandardAppointmentParameters())
            {
                Now = TestDateTime
            };
        }
    }
}
