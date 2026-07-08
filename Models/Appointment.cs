using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazeCareWebApi.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly PreferedTime { get; set; }
        public string SymptomsDescription { get; set; } = string.Empty;
        public string NatureOfVisit { get; set; } = null!; 
        public string Status { get; set; } = "Requested"; 
        public MedicalRecords? MedicalRecord { get; set; }

    }
}
