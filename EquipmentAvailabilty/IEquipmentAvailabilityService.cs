using System;
using System.Collections.Generic;

namespace EquipmentAvailabilty
{
    public interface IEquipmentAvailabilityService
    {
        IList<EquipmentAvailabilityDto> GetAvailability(DateTime dateTime);
        void SetAvailability(int equipmentId, DateTime dateTime, bool isAvailable);
    }
}
