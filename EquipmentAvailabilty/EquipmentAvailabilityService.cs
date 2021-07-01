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

        public void SetAvailability(EquipmentAvailabilityDto equipmentAvailabilityDto)
        {
            if(_equipmentItems.Any(e => e.Id == equipmentAvailabilityDto.EquipmentId) == false)
            {
                throw new ArgumentOutOfRangeException("equipmentId", $"equipmentId '{equipmentAvailabilityDto.EquipmentId}' does not exist");
            }

            var booking = _equipmentItemBookings.Where(a => a.EquipmentId == equipmentAvailabilityDto.EquipmentId && a.Date == equipmentAvailabilityDto.Date).SingleOrDefault();

            if(booking == null)
            {
                if(equipmentAvailabilityDto.IsAvailable == false)
                {
                    _equipmentItemBookings.Add(new EquipmentItemBooking
                    {
                        EquipmentId = equipmentAvailabilityDto.EquipmentId,
                        Date = equipmentAvailabilityDto.Date
                    });
                }
            }
            else
            {
                if (equipmentAvailabilityDto.IsAvailable == true)
                {
                    _equipmentItemBookings.Remove(booking);
                }
            }
        }
    }
}
