using AmazeCareWebApi.Data;
using AmazeCareWebApi.Migrations;
using AmazeCareWebApi.Models;
using AmazeCareWebApi.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AmazeCareWebApi.Repository.Implementation
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly AppoinmentDbContext _context;

        public PrescriptionRepository(AppoinmentDbContext context)
        {
            _context = context;
        }


        public async Task AddPrescriptionAsync(Prescriptions prescriptions)
        {
            await _context.Prescriptions.AddAsync(prescriptions);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePrescriptionByIdAsync(int PrescriptionId)
        {
            await _context.Prescriptions.Where(u => u.PrescriptionId == PrescriptionId).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }

        public async Task<List<Prescriptions>> GetAllPrescriptionsAsync()
        {
            return await _context.Prescriptions
            .Include(p => p.MedicalRecord)
            .ToListAsync();
        }

        public async Task<List<Prescriptions>?> GetPrescriptionByRecordIdAsync(int RecordId)
        {
            return await _context.Prescriptions
            .Include(p => p.MedicalRecord)
            .Where(a => a.RecordId == RecordId)
            .ToListAsync();
        }


        public async Task<Prescriptions?> GetPrescriptionByIdAsync(int PrescriptionId)
        {
            return await _context.Prescriptions
            .Include(p => p.MedicalRecord)
            .FirstOrDefaultAsync(a => a.PrescriptionId == PrescriptionId);
        }

        public async Task UpdatePrescriptionAsync(Prescriptions prescriptions)
        {
            _context.Prescriptions.Update(prescriptions);
            await _context.SaveChangesAsync();
        }

    }
}
