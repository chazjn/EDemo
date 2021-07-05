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
                var booking = _equipmentItemBookings.Where(a => a.EquipmentId == equipmentItem.Id && a.Date == dateTime).FirstOrDefault();

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
            var firstAvailableEquipmentItem = GetAvailability(dateTime).Where(a => a.IsAvailable == true).FirstOrDefault();

            if (firstAvailableEquipmentItem == null)
                return new EquipmentReservationResult();

            var booking = new EquipmentItemBooking
            {
                EquipmentId = firstAvailableEquipmentItem.EquipmentId,
                Date = dateTime
            };

            _equipmentItemBookings.Add(booking);

            return new EquipmentReservationResult
            {
                Successful = true,
                EquipmentAvailability = firstAvailableEquipmentItem
            };
        }

        public void UnreserveEquipment(DateTime dateTime, int equipmentId)
        {
            var booking = _equipmentItemBookings.Where(e => e.Date == dateTime && e.EquipmentId == equipmentId).FirstOrDefault();
            if(booking != null)
            {
                _equipmentItemBookings.Remove(booking);
            }
        }
    }
}
