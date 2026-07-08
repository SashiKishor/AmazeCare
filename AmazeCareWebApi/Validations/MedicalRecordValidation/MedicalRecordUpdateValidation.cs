using AmazeCareWebApi.Dtos.MedicalRecordDtos;
using FluentValidation;

namespace AmazeCareWebApi.Validations.MedicalRecordValidation
{
    public class MedicalRecordUpdateValidation:AbstractValidator<MedicalRecordUpdateDto>
    {
        public MedicalRecordUpdateValidation()
        {
            RuleFor(a => a.RecordId)
                .NotEmpty()
                .WithMessage("Record Id is required.");

            RuleFor(a => a.PhysicalExamination)
                .NotEmpty()
                .WithMessage("Physical Examination of patient is required.");

            RuleFor(a => a.MedicalTest)
               .NotEmpty()
               .WithMessage("Medical Test of patient is required.");

            RuleFor(a => a.CurrentSymptoms)
               .NotEmpty()
               .WithMessage("Current Symptoms of patient is required.");

        }
    }
}
