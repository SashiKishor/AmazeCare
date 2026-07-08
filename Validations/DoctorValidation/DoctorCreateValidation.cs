using AmazeCareWebApi.Dtos.DoctorDtos;
using FluentValidation;

namespace AmazeCareWebApi.Validations.DoctorValidation
{
    public class DoctorCreateValidation:AbstractValidator<DoctorCreateDto>
    {
        public DoctorCreateValidation()
        {
            RuleFor(a => a.DoctorName)
                .NotEmpty()
                .WithMessage("Doctor Name is required");
            
            RuleFor(a => a.Experience)
                .NotEmpty()
                .WithMessage("Doctor Experience is required");

            RuleFor(a => a.Speciality)
                .NotEmpty()
                .WithMessage("Doctor Speciality is required");
            
            RuleFor(a => a.Qualification)
                .NotEmpty()
                .WithMessage("Doctor Qualification is required");
            
            RuleFor(a => a.Designation)
                .NotEmpty()
                .WithMessage("Doctor Designation is required");
        }

    }
}
