using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazeCareWebApi.Models
{
    public class MedicalRecords
    {
        [Key]
        public int RecordId { get; set; }
        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }
        public string CurrentSymptoms { get; set; } = string.Empty; 
        public string PhysicalExamination { get; set; } = string.Empty; 
        public string TreatmentPlan { get; set; } = string.Empty; 
        public string RecommendedTests { get; set; } = string.Empty; 
        public ICollection<Prescriptions>? Prescriptions { get; set; }

    }
}
