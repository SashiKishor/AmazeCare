using AmazeCareWebApi.Dtos.PrescriptionDtos;
using FluentValidation;

namespace AmazeCareWebApi.Validations.PerscriptionValidation
{
    public class PerscriptionUpdateDtoValidation:AbstractValidator<PrescriptionUpdateDto>
    {
        public PerscriptionUpdateDtoValidation()
        {
            RuleFor(a => a.PrescriptionId)
                .NotEmpty()
                .WithMessage("Prescription Record Id is required");
        }
    }
}
