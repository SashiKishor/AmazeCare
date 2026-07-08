using AmazeCareWebApi.Dtos.AppointmentDtos;
using FluentValidation;
using System.Timers;

namespace AmazeCareWebApi.Validations.AppointmentValidations
{
    public class AppointmentRescheduleDtoValidation:AbstractValidator<AppointmentRescheduleDto>
    {
        public AppointmentRescheduleDtoValidation()
        {
            RuleFor(a => a.AppointmentId)
                .NotEmpty()
                .WithMessage("Appointment Id is required");

            RuleFor(a => a.RescheduledSlot)
                .NotEmpty()
                .WithMessage("Appointment Slot is required")
                .Must(ValidAppointmentSlot)
                .WithMessage("Enter a valid Appointment Slot");

        }
        private bool ValidAppointmentSlot(DateTime AppointmentDate)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var appointmentDate=DateOnly.FromDateTime(AppointmentDate);
            var appointmentTime=TimeOnly.FromDateTime(AppointmentDate);
            var startOfDay = new TimeOnly(9, 0);
            var endOfDay = new TimeOnly(18, 0);

            return (appointmentDate >= today && appointmentTime >= startOfDay && appointmentTime <= endOfDay);
        }
    }
}
