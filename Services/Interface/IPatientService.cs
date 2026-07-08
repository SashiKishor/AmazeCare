using AmazeCareWebApi.Dtos;
using AmazeCareWebApi.Dtos.PatientDtos;
using AmazeCareWebApi.Models;
namespace AmazeCareWebApi.Services.Interface
{
    public interface IPatientService
    {
        Task<List<PatientResponceDto>> GetAllPatients();
        Task<(string Message, PatientResponceDto? Data)> GetPatientById(int PatientId);
        Task<(bool Success, string Message, PatientResponceDto? Data)> AddPatient(PatientCreateDto patientCreateDto);
        Task<(bool Success,string Message)> DeletePatientById(int PatientId);
        public Task<(string Message, PatientResponceDto? Data)> UpdatePatient(PatientUpdateDto patients);

    }
}
