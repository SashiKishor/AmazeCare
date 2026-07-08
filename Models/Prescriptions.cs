using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazeCareWebApi.Models
{
    public class Prescriptions
    {
        [Key]
        public int PrescriptionId { get; set; }

        [ForeignKey("MedicalRecord")]
        public int RecordId { get; set; }
        public MedicalRecords? MedicalRecord { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
       
    }
}
