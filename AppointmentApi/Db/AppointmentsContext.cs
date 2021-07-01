using AppointmentApi.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApi.Db
{
    public class AppointmentsContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        public AppointmentsContext(DbContextOptions options) : base(options)
        {
        }
    }
}
