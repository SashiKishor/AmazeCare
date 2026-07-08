using AmazeCareWebApi.Dtos.PrescriptionDtos;
using AmazeCareWebApi.Models;

namespace AmazeCareWebApi.Services.Interface
{
    public interface IPerscriptionService
    {
        public Task<(bool sucess, string message)> AddPrescription(PrescriptionCreateDto prescriptions);
        public Task<(bool sucess, string message, PrescriptionResponceDto? data)> GetPrescriptionById(int id);
        public Task<(bool sucess, string message, PrescriptionResponceDto? data)> UpdatePrescription(PrescriptionUpdateDto prescriptions);
        public Task<(bool sucess, string message,List<PrescriptionResponceDto>? data)> GetPrescriptionsByRecordId(int recordId);
    }
}
