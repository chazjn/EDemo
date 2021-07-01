using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppointmentApi.Db.Models
{
    public class Patient
    {
        public int Id { get; set; }
        
        [Required, MaxLength(255)]
        public string EmailAddress { get; set; }

        public List<Appointment> Appointments { get; set; }
    }
}
