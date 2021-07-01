using System;

namespace EquipmentAvailabilty
{
    public class EquipmentAvailabilityDto
    {
        public int EquipmentId { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime Date { get; set; }
    }
}
