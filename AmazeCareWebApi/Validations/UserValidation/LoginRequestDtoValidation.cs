using AmazeCareWebApi.Dtos.User;
using FluentValidation;

namespace AmazeCareWebApi.Validations.UserValidation
{
    public class LoginRequestDtoValidation:AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidation()
        {
            RuleFor(a => a.UserName)
                .NotEmpty()
                .WithMessage("User Name is Required.");
            
            RuleFor(a => a.Password)
                .NotEmpty()
                .WithMessage("User Password is Required.");
        }
    }
}
