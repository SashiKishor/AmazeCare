
using AmazeCareWebApi.Models;

namespace AmazeCareWebApi.Repository.Interface
{
    public interface IMedicalRecordRepository
    {
        Task AddMedicalRecordsAsync(MedicalRecords medicalRecord);
        Task<List<MedicalRecords>> GetAllMedicalRecordsAsync();
        Task DeleteMedicalRecordAsync(int medicalRecordId);
        Task<MedicalRecords?> GetMedicalRecordByIdAsync(int medicalRecordId);
        Task UpdateMedicalRecordAsync(MedicalRecords medicalRecords);
    }
}
