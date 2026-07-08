using AmazeCareWebApi.Data;
using AmazeCareWebApi.Models;
using AmazeCareWebApi.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AmazeCareWebApi.Repository.Implementation
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppoinmentDbContext _context;

        public PatientRepository(AppoinmentDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task AddPatientAsync(Patient patient)
        {
            await _context.patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePatientByIdAsync(int PatientId)
        {
            await _context.patients.Where(u => u.PatientId == PatientId).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }
        public async Task<List<Patient>> GetAllPatientsAsync()
        {
            return await _context.patients.ToListAsync();
        }

        public async Task<Patient?> GetPatientByIdAsync(int PatientId)
        {
            return await _context.patients.FirstOrDefaultAsync(a=>a.PatientId==PatientId);
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            _context.patients.Update(patient);
            await _context.SaveChangesAsync();
        }

    }
}
