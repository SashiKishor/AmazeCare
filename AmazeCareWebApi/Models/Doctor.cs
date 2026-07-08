using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace AmazeCareWebApi.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = null!;
        public string Speciality { get; set; } = null!; 
        public float Experience { get; set; }
        public string Qualification { get; set; } = null!; 
        public string Designation { get; set; } = null!; 
        public ICollection<Appointment>? Appointments { get; set; }

        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
