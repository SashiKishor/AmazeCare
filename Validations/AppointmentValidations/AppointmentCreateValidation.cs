using AmazeCareWebApi.Dtos.AppointmentDtos;
using FluentValidation;

namespace AmazeCareWebApi.Validations.AppointmentValidations
{
    public class AppointmentCreateValidation:AbstractValidator<AppointmentCreateDto>
    {
        private readonly List<string> _allowedStatuses = new() { "Requested", "Upcoming", "Active", "Completed" };
        public AppointmentCreateValidation()
        {
            RuleFor(a => a.PatientId)
                .NotEmpty()
                .WithMessage("Patient Id is required");

            RuleFor(a => a.DoctorId)
                .NotEmpty()
                .WithMessage("Doctor Id is required");

            RuleFor(x => x.Status)
               .Must(ValidStatus)
               .WithMessage("Starus can be a Requested,Upcoming,Active,Completed");

            RuleFor(a => a.NatureOfVisit)
                .NotEmpty()
                .WithMessage("Nature of the visit is required");

            RuleFor(a => a.AppointmentDate)
                .NotEmpty()
                .WithMessage("Appointment Date is required")
                .Must(ValidAppointmentDate)
                .WithMessage("Enter a valid Appointment date");

            RuleFor(a => a.PreferedTime)
                .NotEmpty()
                .WithMessage("Prefered Time is required")
                .Must(ValidAppointmentTime)
                .WithMessage("Enter a valid Appointment Time");

        }
        private bool ValidAppointmentDate(DateOnly AppointmentDate)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return AppointmentDate >= today;
        }
        private bool ValidAppointmentTime(TimeOnly time)
        {
            var startOfDay = new TimeOnly(9, 0);  
            var endOfDay = new TimeOnly(18, 0);   
            return time >= startOfDay && time <= endOfDay;
        }
        private bool ValidStatus(string status)
        {
            return _allowedStatuses.Contains(status);
        }
    }
}
