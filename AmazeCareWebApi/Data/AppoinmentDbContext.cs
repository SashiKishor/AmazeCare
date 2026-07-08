using AmazeCareWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AmazeCareWebApi.Data
{
    public class AppoinmentDbContext:DbContext
    {

        public AppoinmentDbContext(DbContextOptions options):base(options)
        {
            

        }
        
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> patients { get; set; }
        public DbSet<MedicalRecords> medicalRecords { get; set; }
        public DbSet<Prescriptions> Prescriptions { get; set; }

        public DbSet<User> Users { get; set; }


    }
}
