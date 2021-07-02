using System;
using System.Collections.Generic;

namespace EquipmentAvailabilty
{
    public interface IEquipmentAvailabilityService
    {
        IList<EquipmentAvailability> GetAvailability(DateTime dateTime);
        EquipmentReservationResult ReserveEquipment(DateTime dateTime);
        void UnreserveEquipment(DateTime dateTime);
    }
}
