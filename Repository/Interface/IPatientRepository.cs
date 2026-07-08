using AmazeCareWebApi.Models;

namespace AmazeCareWebApi.Repository.Interface
{
    public interface IPatientRepository
    {
        Task AddPatientAsync(Patient patient);
        Task<List<Patient>> GetAllPatientsAsync();
        Task DeletePatientByIdAsync(int PatientId);
        Task<Patient?> GetPatientByIdAsync(int PatientId);
        Task UpdatePatientAsync(Patient patient);
    }
}
