using AmazeCareWebApi.Models;

namespace AmazeCareWebApi.Repository.Interface
{
    public interface IDoctorRepository
    {
        Task AddDoctorAsync(Doctor doctor);
        Task<List<Doctor>> GetAllDoctorsAsync();
        Task DeleteDoctorByIdAsync(int DoctorId);
        Task<Doctor?> GetDoctorByIdAsync(int DoctorId);
        IQueryable<Doctor> GetAllDoctorsQueryable();
        Task UpdateDoctorRecordAsync(Doctor doctor);
    }
}
