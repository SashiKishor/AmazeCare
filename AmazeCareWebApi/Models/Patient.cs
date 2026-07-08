using System.ComponentModel.DataAnnotations.Schema;

namespace AmazeCareWebApi.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string FullName { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public ICollection<Appointment>? Appointments { get; set; }

        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
