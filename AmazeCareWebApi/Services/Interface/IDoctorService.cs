using AmazeCareWebApi.Dtos.DoctorDtos;

namespace AmazeCareWebApi.Services.Interface
{
    public interface IDoctorService
    {
        Task<List<DoctorResponceDto>> GetAllDoctorsAsync();
        Task<(string Message, DoctorResponceDto? Data)> GetDoctorByIdAsync(int DoctorId);
        Task<(bool Success, string Message, DoctorResponceDto? Data)> AddDoctorAsync(DoctorCreateDto doctorCreateDto);
        Task<(bool Success, string Message)> DeleteDoctorByIdAsync(int DoctorId);
        Task<(bool Sucess, string Message, List<DoctorResponceDto>? data)> GetAvailableDoctorsAsync(DoctorAvailabiltyRequest request);
        Task<(bool Success, string Message, DoctorResponceDto? data)> UpdateDoctorDetailsAsync(DoctorUpdateDto doctorUpdateDto);
    }
}
