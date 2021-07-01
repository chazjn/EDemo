using System;

namespace AppointmentApi.Db.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int EquipmentId { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsDeleted { get; set; }

        public Patient Patient { get; set; }
    }
}
