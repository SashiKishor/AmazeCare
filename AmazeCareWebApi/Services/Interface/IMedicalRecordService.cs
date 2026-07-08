using AmazeCareWebApi.Dtos.DoctorDtos;
using AmazeCareWebApi.Dtos.MedicalRecordDtos;
using AmazeCareWebApi.Dtos.PatientDtos;

namespace AmazeCareWebApi.Services.Interface
{
    public interface IMedicalRecordService
    {
        Task<(bool Success, string Message,MedicalRecordResponceDto? Data)> GetMedicalRecordByIdAsync(int recordId);
        Task<(bool Success, string Message)> AddMedicalRecordAsync(MedicalRecordCreateDto medicalRecordCreate);
        Task<(bool Success, string Message, MedicalRecordResponceDto? Data)> UpdateMedicalRecord(MedicalRecordUpdateDto updateDto);

    }
}
