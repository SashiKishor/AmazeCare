using AmazeCareWebApi.Dtos.AppointmentDtos;
using FluentValidation;

namespace AmazeCareWebApi.Validations.AppointmentValidations
{
    public class AppointmentUpdateStatusDtoValidation : AbstractValidator<AppointmentUpdateStatusDto>
    {
        private readonly List<string> _allowedStatuses = new() { "Requested", "Upcoming", "Active", "Completed", "Cancelled", "Rescheduled" };
        public AppointmentUpdateStatusDtoValidation()
        {
            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("Appointment Status is Required.")
                .Must(ValidStatus)
                .WithMessage("Status can be a Requested, Upcoming, Active, Completed, Cancelled, or Rescheduled");

            RuleFor(x => x.AppointmentId).NotEmpty().WithMessage("Appointment Id is Required");
        }

        private bool ValidStatus(string status)
        {
            return _allowedStatuses.Contains(status);
        }
    }
}
