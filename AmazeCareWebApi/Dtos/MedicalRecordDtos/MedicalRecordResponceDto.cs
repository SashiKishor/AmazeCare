using AmazeCareWebApi.Dtos.AppointmentDtos;

namespace AmazeCareWebApi.Dtos.MedicalRecordDtos
{
    public class MedicalRecordResponceDto
    {
        public int RecordId { get; set; }
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public DateOnly? AppointmentDate { get; set; }
        public string CurrentSymptoms { get; set; } = null!;
        public string PhysicalExamination { get; set; } = null!;
        public string TreatmentPlan { get; set; } = string.Empty;
        public string MedicalTest { get; set; } = null!;
        public List<ReportPerscriptionDTo> Prescriptions { get; set; } = new();
    }
}
