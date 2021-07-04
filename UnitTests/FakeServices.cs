using System;
using System.Collections.Generic;
using System.Text;
using AppointmentApi.Db;
using EquipmentAvailabiltySystem;
using EmailNotificationSystem;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace UnitTests
{
    public class FakeServices
    {
        public static AppointmentsContext AppointmentsContext
        {
            get
            {
                var optionsBuilder = new DbContextOptionsBuilder<AppointmentsContext>();
                optionsBuilder.UseInMemoryDatabase("InMemoryDb");
                var dbContext = new AppointmentsContext(optionsBuilder.Options);
                return dbContext;
            }
        }

        public static IEquipmentAvailabilityService EquipmentAvailabilityService_WithAvailabilityOn(DateTime dateTime)
        {
            var equipmentAvailability = new List<EquipmentAvailability>
            {
                new EquipmentAvailability
                {
                    EquipmentId = 1,
                    IsAvailable = true,
                    Date = dateTime
                },
                new EquipmentAvailability
                {
                    EquipmentId = 2,
                    IsAvailable = true,
                    Date = dateTime
                }
            };

            var service = Substitute.For<IEquipmentAvailabilityService>();
            service.GetAvailability(dateTime).Returns(equipmentAvailability);
            return service;
        }

        public static IEquipmentAvailabilityService EquipmentAvailabilityService_WithoutAvailabilityOn(DateTime dateTime)
        {
            var equipmentAvailability = new List<EquipmentAvailability>
            {
                new EquipmentAvailability
                {
                    EquipmentId = 1,
                    IsAvailable = false,
                    Date = dateTime
                },
                new EquipmentAvailability
                {
                    EquipmentId = 2,
                    IsAvailable = false,
                    Date = dateTime
                }
            };

            var service = Substitute.For<IEquipmentAvailabilityService>();
            service.GetAvailability(dateTime).Returns(equipmentAvailability);
            return service;
        }

        public static IEquipmentAvailabilityService EquipmentAvailabilityService_WithMixedAvailabilityOn(DateTime dateTime)
        {
            var equipmentAvailability = new List<EquipmentAvailability>
            {
                new EquipmentAvailability
                {
                    EquipmentId = 1,
                    IsAvailable = false,
                    Date = dateTime
                },
                new EquipmentAvailability
                {
                    EquipmentId = 2,
                    IsAvailable = true,
                    Date = dateTime
                }
            };

            var service = Substitute.For<IEquipmentAvailabilityService>();
            service.GetAvailability(dateTime).Returns(equipmentAvailability);
            return service;
        }

        public static ISmtpClient SmtpClient
        {
            get
            {
                var client = Substitute.For<ISmtpClient>();
                return client;
            }
        }
    }
}
