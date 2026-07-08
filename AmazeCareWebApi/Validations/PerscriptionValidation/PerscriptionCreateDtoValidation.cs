using AmazeCareWebApi.Dtos.PrescriptionDtos;
using FluentValidation;

namespace AmazeCareWebApi.Validations.PerscriptionValidation
{
    public class PerscriptionCreateDtoValidation:AbstractValidator<PrescriptionCreateDto>
    {
        public PerscriptionCreateDtoValidation()
        {
            RuleFor(a => a.RecordId)
                .NotEmpty()
                .WithMessage("Medical Record Id is required");
        }
    }
}
