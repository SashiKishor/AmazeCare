namespace AmazeCareWebApi.Dtos.PrescriptionDtos
{
    public class PrescriptionCreateDto
    {
        public int RecordId { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
    }
}
