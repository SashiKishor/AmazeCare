using AmazeCareWebApi.Data;
using AmazeCareWebApi.Models;
using AmazeCareWebApi.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AmazeCareWebApi.Repository.Implementation
{
    public class AppotimentRepository:IAppointmentRepository
    {
        private readonly AppoinmentDbContext _context;

        public AppotimentRepository(AppoinmentDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<bool> AddAppointmentRecordsAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> AppointmentExistsAsync(int appointmentId)
        {
            return await _context.Appointments.AnyAsync(a=>a.AppointmentId == appointmentId);
        }

        public async Task<bool> DeleteAppointmentAsync(int appointmentId)
        {
            if(await AppointmentExistsAsync(appointmentId))
            {
                await _context.Appointments.Where(a=>a.AppointmentId==appointmentId).ExecuteDeleteAsync();
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments
                .Include(a=>a.Doctor)
                .Include(a=>a.Patient)
                .Include(a=>a.MedicalRecord)
                .ToListAsync();
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(int appointmentId)
        {
            if (await AppointmentExistsAsync(appointmentId))
            {
               return await _context.Appointments
                    .Include(a=>a.Doctor)
                    .Include(a=>a.Patient)
                    .Include(a=>a.MedicalRecord)
                    .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
            }
            return null;
        }

        public IQueryable<Appointment> GetAppointmentRecordsQueryable()
        {
           return _context.Appointments
                  .Include(a=>a.Patient)
                  .Include(a=>a.Doctor)
                  .Include(a=>a.MedicalRecord)
                  .AsQueryable();
        }

        public async Task UpdateAppointment(Appointment appointment)
        {
            _context.Update(appointment);
            await _context.SaveChangesAsync();
        }
    }
}
