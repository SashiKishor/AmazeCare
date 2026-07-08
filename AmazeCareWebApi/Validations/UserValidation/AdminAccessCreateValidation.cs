using AmazeCareWebApi.Dtos.User;
using FluentValidation;

namespace AmazeCareWebApi.Validations.UserValidation
{
    public class AdminAccessCreateValidation:AbstractValidator<AdminAccessCreateDto>
    {
        private readonly List<string> _allowedRoles=new() {"Doctor","Patient","Admin"};
        public AdminAccessCreateValidation()
        {
            RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full Name is required.");

            RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full Name is required.");

            RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.");

            RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Please enter a valid email address.");

            RuleFor(x => x.Role)
                .Must(ValidRole)
                .WithMessage("Enter an valid Role.");

            RuleFor(x => x.Speciality)
                .NotEmpty()
                .When(x => x.Role == "Doctor")
                .WithMessage("Specialty is required for doctors.");

            RuleFor(x => x.Experience)
                .NotNull()
                .GreaterThanOrEqualTo(0)
                .When(x => x.Role == "Doctor")
                .WithMessage("Experience is required and must be non-negative.");

            RuleFor(x => x.Qualification)
                .NotEmpty()
                .When(x => x.Role == "Doctor")
                .WithMessage("Qualification is required for doctors.");

            RuleFor(x => x.Designation)
                .NotEmpty()
                .When(x => x.Role == "Doctor")
                .WithMessage("Designation is required for doctors.");
        }

        private bool ValidRole(string role)
        {
            return _allowedRoles.Contains(role);
        }

    }
}
