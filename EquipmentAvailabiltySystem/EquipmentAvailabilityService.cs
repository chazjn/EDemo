using System;
using System.Linq;
using System.Collections.Generic;
using EquipmentAvailabiltySystem.Models;

namespace EquipmentAvailabiltySystem
{
    public class EquipmentAvailabilityService : IEquipmentAvailabilityService
    {
        private readonly IList<EquipmentItem> _equipmentItems;
        private readonly IList<EquipmentItemBooking> _equipmentItemBookings;

        public EquipmentAvailabilityService()
        {
            _equipmentItems = new List<EquipmentItem>
            {
                new EquipmentItem { Id = 1 },
                new EquipmentItem { Id = 2 },
                new EquipmentItem { Id = 3 }
            };

            _equipmentItemBookings = new List<EquipmentItemBooking>();
        }

        public IList<EquipmentAvailability> GetAvailability(DateTime dateTime)
        {
            var list = new List<EquipmentAvailability>();

            foreach (var equipmentItem in _equipmentItems)
            {
                var booking = _equipmentItemBookings.Where(a => a.EquipmentId == equipmentItem.Id && a.Date == dateTime).SingleOrDefault();

                list.Add(new EquipmentAvailability
                {
                    EquipmentId = equipmentItem.Id,
                    Date = dateTime,
                    IsAvailable = booking == null
                });
            }

            return list;
        }

        public EquipmentReservationResult ReserveEquipment(DateTime dateTime)
        {
            var availability = GetAvailability(dateTime);
            var result = new EquipmentReservationResult();

            if (availability.Count > 0)
            {
                availability.First().IsAvailable = false;
                return new EquipmentReservationResult
                {
                    Successful = true,
                    EquipmentAvailability = availability.First()
                };
            }

            return result;
        }

        public void UnreserveEquipment(DateTime dateTime)
        {
            var availability = GetAvailability(dateTime);

            if (availability.Count > 0)
            {
                var item = availability.FirstOrDefault(a => a.IsAvailable == false);
                if(item != null)
                {
                    item.IsAvailable = true;
                }
            }
        }
    }
}
