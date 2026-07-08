using AmazeCareWebApi.Data;
using AmazeCareWebApi.Models;
using AmazeCareWebApi.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AmazeCareWebApi.Repository.Implementation
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly AppoinmentDbContext _context;

        public DoctorRepository(AppoinmentDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task AddDoctorAsync(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteDoctorByIdAsync(int DoctorId)
        {
            await _context.Doctors.Where(u => u.DoctorId == DoctorId).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }
        public async Task<List<Doctor>> GetAllDoctorsAsync()
        {
            return await _context.Doctors.ToListAsync();
        }

        public async Task<Doctor?> GetDoctorByIdAsync(int DoctorId)
        {
            return await _context.Doctors.FirstOrDefaultAsync(a => a.DoctorId == DoctorId);
        }

        public IQueryable<Doctor> GetAllDoctorsQueryable()
        {
            return _context.Doctors.AsQueryable();
        }


        public async Task UpdateDoctorRecordAsync(Doctor Doctor)
        {
            _context.Doctors.Update(Doctor);
            await _context.SaveChangesAsync();
        }


    }
}
