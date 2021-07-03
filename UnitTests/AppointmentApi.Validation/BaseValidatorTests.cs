using AppointmentApi.Db;
using AppointmentApi.Db.Models;
using AppointmentApi.Validation;
using EquipmentAvailabilty;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.AppointmentApi.Validation
{
    public abstract class BaseValidatorTests
    {
        protected AppointmentsContext AppointmentsContext { get; set; }
        protected IEquipmentAvailabilityService EquipmentAvailabilityService { get; set; }
        protected DateTime ValidAppointmentDateTime { get; set; }
        protected DateTime InvalidAppointmentDateTime { get; set; }

        [SetUp]
        public void SetUp()
        {
            AppointmentsContext = FakeServices.AppointmentsContext;
            ValidAppointmentDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0);
            InvalidAppointmentDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 0, 0);
        }

        [TearDown]
        public void TearDown()
        {
            AppointmentsContext.Database.EnsureDeleted();
        }

        protected void SeedDatabase(int numberOfPatients)
        {
            for (int i = 0; i < numberOfPatients; i++)
            {
                AppointmentsContext.Patients.Add(new Patient { Id = i, EmailAddress = $"email_{i}@example.com" });
            }
            AppointmentsContext.SaveChanges();
        }

        protected CreateAppointmentValidator GetValidator()
        {
            return new CreateAppointmentValidator(new StandardAppointmentParameters(),
                                                  new AppointmentsRepository(AppointmentsContext),
                                                  EquipmentAvailabilityService);
        }
    }
}
