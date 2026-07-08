using AmazeCareWebApi.Dtos.PatientDtos;
using FluentValidation;

namespace AmazeCareWebApi.Validations.patientValidation
{
    public class PatientUpdateDtoValidation : AbstractValidator<PatientUpdateDto>
    {
        public PatientUpdateDtoValidation()
        {
            RuleFor(a => a.PatientId)
                .NotEmpty()
                .WithMessage("Patient Id is required.");

            RuleFor(a => a.FullName)
                .NotEmpty()
                .WithMessage("Full name is required.");

            RuleFor(a => a.Gender)
                .NotEmpty()
                .WithMessage("Gender is required.");
            
            RuleFor(x => x.DateOfBirth)
                        .NotEmpty()
                        .WithMessage("Date Of Birth is required.")
                        .Must(ValidDob)
                        .WithMessage("Patient date of birth cannot be in future.");

            RuleFor(a => a.ContactNumber)
               .NotEmpty()
               .WithMessage("Contact Number is required.")
               .Matches(@"^[6-9]\d{9}$")
               .WithMessage("Please enter a valid 10-digit phone number.");
        }

        private bool ValidDob(DateOnly dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return dateOfBirth <= today;
        }
    }
}
