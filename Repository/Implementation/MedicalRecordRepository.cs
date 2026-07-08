using AmazeCareWebApi.Data;
using AmazeCareWebApi.Models;
using AmazeCareWebApi.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AmazeCareWebApi.Repository.Implementation
{
    public class MedicalRecordRepository:IMedicalRecordRepository
    {
        private readonly AppoinmentDbContext _context;

        public MedicalRecordRepository(AppoinmentDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task AddMedicalRecordsAsync(MedicalRecords medicalRecord)
        {
            await _context.medicalRecords.AddAsync(medicalRecord);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteMedicalRecordAsync(int medicalRecordId)
        {
                await _context.medicalRecords.Where(u => u.RecordId == medicalRecordId).ExecuteDeleteAsync();
                await _context.SaveChangesAsync();
          
        }

        public async Task<List<MedicalRecords>> GetAllMedicalRecordsAsync()
        {
            return await _context.medicalRecords
                .Include(a => a.Appointment)
                .ThenInclude(a => a.Doctor)
                .Include(a => a.Appointment)
                .ThenInclude(a => a.Patient)
                .Include(a=>a.Prescriptions)
                .ToListAsync();
        }

        public async Task<MedicalRecords?> GetMedicalRecordByIdAsync(int medicalRecordId)
        {
            return await _context.medicalRecords
                .Include(a => a.Appointment)
                .ThenInclude(a=>a.Doctor)
                .Include(a => a.Appointment)
                .ThenInclude(a => a.Patient)
                .Include(a => a.Prescriptions)
                .FirstOrDefaultAsync(u => u.RecordId == medicalRecordId);
        }

        public async Task UpdateMedicalRecordAsync(MedicalRecords medicalRecords)
        {
            _context.medicalRecords.Update(medicalRecords);
            await _context.SaveChangesAsync();
        }



    }
}
