using AmazeCareWebApi.Dtos.AppointmentDtos;
using AmazeCareWebApi.Dtos.DoctorDtos;
using AmazeCareWebApi.Dtos.MedicalRecordDtos;
using AmazeCareWebApi.Dtos.PatientDtos;
using AmazeCareWebApi.Models;
using AutoMapper;

namespace AmazeCareWebApi.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<AppointmentCreateDto, Appointment>()
                .ForMember(dest=>dest.AppointmentId,opt=>opt.Ignore());
            CreateMap<PatientCreateDto, Patient>()
               .ForMember(dest => dest.PatientId, opt => opt.Ignore());
            CreateMap<DoctorCreateDto, Doctor>()
               .ForMember(dest => dest.DoctorId, opt => opt.Ignore());
            CreateMap<MedicalRecordCreateDto, MedicalRecords>()
               .ForMember(dest => dest.RecordId, opt => opt.Ignore());

            CreateMap<Appointment,AppointmentResponceDto>();
            CreateMap<Patient, PatientResponceDto>();
            CreateMap<Doctor, DoctorResponceDto>();
            CreateMap<MedicalRecords, MedicalRecordResponceDto>();

        }
    }
}
