using System;
using System.Linq;
using System.Collections.Generic;
using EquipmentAvailabilty.Models;

namespace EquipmentAvailabilty
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

        public IList<EquipmentAvailabilityDto> GetAvailability(DateTime dateTime)
        {
            var list = new List<EquipmentAvailabilityDto>();

            foreach (var equipmentItem in _equipmentItems)
            {
                var booking = _equipmentItemBookings.Where(a => a.EquipmentId == equipmentItem.Id && a.Date == dateTime).SingleOrDefault();

                list.Add(new EquipmentAvailabilityDto
                {
                    EquipmentId = equipmentItem.Id,
                    Date = dateTime,
                    IsAvailable = booking == null
                });
            }

            return list;
        }

        public void SetAvailability(int equipmentId, DateTime dateTime, bool isAvailable)
        {
            if(_equipmentItems.Any(e => e.Id == equipmentId) == false)
            {
                throw new ArgumentOutOfRangeException("equipmentId", $"equipmentId '{equipmentId}' does not exist");
            }

            var booking = _equipmentItemBookings.Where(a => a.EquipmentId == equipmentId && a.Date == dateTime).SingleOrDefault();

            if(booking == null)
            {
                if(isAvailable == false)
                {
                    _equipmentItemBookings.Add(new EquipmentItemBooking
                    {
                        EquipmentId = equipmentId,
                        Date = dateTime
                    });
                }
            }
            else
            {
                if (isAvailable == true)
                {
                    _equipmentItemBookings.Remove(booking);
                }
            }
        }
    }
}
