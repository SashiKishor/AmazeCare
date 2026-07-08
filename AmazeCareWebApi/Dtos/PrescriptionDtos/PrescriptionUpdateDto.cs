namespace AmazeCareWebApi.Dtos.PrescriptionDtos
{
    public class PrescriptionUpdateDto
    {
        public int PrescriptionId { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
    }
}
