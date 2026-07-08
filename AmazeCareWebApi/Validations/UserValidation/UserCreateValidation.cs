using AmazeCareWebApi.Dtos.User;
using FluentValidation;

namespace AmazeCareWebApi.Validations.UserValidation
{
    public class UserCreateValidation:AbstractValidator<UserCreateDto>
    {
        public UserCreateValidation()
        {
            RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full Name is required.");

            RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full Name is required.");

            RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]")
            .WithMessage("Password must contain at least one number.");

            RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Please enter a valid email address.");

            RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .WithMessage("Date of Birth is required.");

            RuleFor(x => x.Gender)
            .NotEmpty()
            .WithMessage("Gender is required.");

            RuleFor(x => x.ContactNumber)
            .NotEmpty()
            .WithMessage("Contact Number is required.")
            .Matches(@"^\d{10}$")
            .WithMessage("Contact Number must be a valid 10-digit number.");
        }
    }
}
