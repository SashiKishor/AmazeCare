namespace AmazeCareWebApi.Dtos.MedicalRecordDtos
{
    public class MedicalRecordUpdateDto
    {
        public int RecordId { get; set; }
        public string CurrentSymptoms { get; set; } = null!;
        public string PhysicalExamination { get; set; } = null!;
        public string TreatmentPlan { get; set; } = string.Empty;
        public string MedicalTest { get; set; } = null!;
    }
}
