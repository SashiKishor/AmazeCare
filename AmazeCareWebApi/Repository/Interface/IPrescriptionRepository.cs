using AmazeCareWebApi.Models;

namespace AmazeCareWebApi.Repository.Interface
{
    public interface IPrescriptionRepository
    {
        Task AddPrescriptionAsync(Prescriptions prescriptions);
        Task<List<Prescriptions>> GetAllPrescriptionsAsync();
        Task DeletePrescriptionByIdAsync(int PrescriptionId);
        Task<Prescriptions?> GetPrescriptionByIdAsync(int PrescriptionId);
        Task<List<Prescriptions>?> GetPrescriptionByRecordIdAsync(int RecordId);
        Task UpdatePrescriptionAsync(Prescriptions prescriptions);

    }
}
