using Microsoft.EntityFrameworkCore;
using therapy_app_backend.DAL.Entities;
using therapy_app_backend.DAL.Interfaces;

namespace therapy_app_backend.DAL.Repositories
{
    public class SpecialtyRepository : ISpecialtyRepository
    {
        private readonly AppDbContext _db;
        public SpecialtyRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task AddSpecialty(Specialty specialty)
        {
            _db.Specialties.Add(specialty);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteSpecialty(Specialty specialty)
        {
            _db.Specialties.Remove(specialty);
            await _db.SaveChangesAsync();
        }
        public async Task<Specialty?> GetById(int id)
        {
            var specialty = await _db.Specialties.FirstOrDefaultAsync(s => s.Id == id);
            if(specialty == null)
            { 
                return null; 
            }
               
            else {
                return specialty;
            }
        }
        public async Task<Specialty?> GetByName(string name)
        {
            var specialty = await _db.Specialties.FirstOrDefaultAsync(s => s.Name == name);
            if (specialty == null)
            {
                return null;
            }

            else
            {
                return specialty;
            }
        }
        public async Task<List<Specialty>> GetAllSpecialties()
        {
            var specialties = await _db.Specialties.ToListAsync();
            return specialties;
           
        }
        public async Task AddTherapistSpecialty(int therapistId, int specialtyId)
        {
            var therapistSpecialty = new TherapistSpecialty
            {
                TherapistId = therapistId,
                SpecialtyId = specialtyId
            };
            _db.TherapistSpecialties.Add(therapistSpecialty);
            await _db.SaveChangesAsync();
        }
        

    }
}
