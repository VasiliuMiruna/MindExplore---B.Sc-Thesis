using Microsoft.EntityFrameworkCore;
using therapy_app_backend.DAL.Entities;
using therapy_app_backend.DAL.Interfaces;

namespace therapy_app_backend.DAL.Repositories
{
    public class TherapistRepository : ITherapistRepository
    {
        private readonly AppDbContext _db;

        public TherapistRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task Create(Therapist therapist)
        {
            await _db.Therapists.AddAsync(therapist);
            await _db.SaveChangesAsync();
        }


        public async Task Update(Therapist therapist)
        {
            _db.Therapists.Update(therapist);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Therapist>> GetAll()
        {
            return _db.Therapists.ToList();

        }
        public async Task<Therapist> GetById(int id) 
        { 
            var therapist = await _db.Therapists.FirstOrDefaultAsync(p => p.TherapistId == id);
            return therapist;
        }
        public async Task DeleteTherapist(Therapist therapist)
        {
            _db.Therapists.Remove(therapist);
            await _db.SaveChangesAsync();

        }
       public async Task<List<Patient>> GetPatientsByTherapistId(int therapistId)
       {
            var patients = await _db.Appointments
                .Where(a => a.TherapistId == therapistId)
                .Select(a => a.Patient)
                .Distinct()
                .ToListAsync();
            return patients;
       }
        public async Task<List<int>> GetSpecialtiesByTherapistId(int therapistId)
        {
            var specialties = await _db.TherapistSpecialties
                .Where(a => a.TherapistId == therapistId)
                .Select(s => s.SpecialtyId)
                .ToListAsync();
            return specialties;
        }

        //update specialties of a therapist
        public async Task UpdateSpecialtiesOfTherapist(int therapistId, List<int> newSpecialties)
        {
            var therapistSpecialties = _db.TherapistSpecialties
                .Where(t => t.TherapistId == therapistId);
            _db.TherapistSpecialties.RemoveRange(therapistSpecialties);
            foreach (var newSpecialty in newSpecialties)
            {
                var newTherapistSpecialty = new TherapistSpecialty
                {
                    TherapistId = therapistId,
                    SpecialtyId = newSpecialty
                };
                _db.TherapistSpecialties.Add(newTherapistSpecialty);
            }
            await _db.SaveChangesAsync();
        }

        //filter by specialty
        public async Task<List<Therapist>> FilterBySpecialties(List<int> specializationIds)
        {
            var therapists = await _db.Therapists
                .Include(t => t.TherapistSpecialties)
                    .ThenInclude(ts => ts.Specialty)
                .Where(t => t.TherapistSpecialties
                    .Any(ts => specializationIds.Contains(ts.SpecialtyId)))
                .ToListAsync();

            return therapists;
        }

        //filter by ascending price
        public async Task<List<Therapist>> FilterByAscendingPrice()
        {
            var therapists = await _db.Therapists
                .OrderBy(t => t.Price)
                .ToListAsync();
            return therapists;
        }

        //filter by specialties and ascending price 
        public async Task<List<Therapist>>FilterBySpecialtiesAndAscendingPrice(List<int> specializationIds)
        {
            var therapists = await _db.Therapists
                .Include(t => t.TherapistSpecialties)
                    .ThenInclude(ts => ts.Specialty)
                .Where(t => t.TherapistSpecialties
                    .Any(ts => specializationIds.Contains(ts.SpecialtyId)))
                .OrderBy(t => t.Price)
                .ToListAsync();

            return therapists;
        }

        //filter by descending price

        public async Task<List<Therapist>> FilterByDescendingPrice()
        {
            var therapists = await _db.Therapists
                .OrderByDescending(t => t.Price)
                .ToListAsync();
            return therapists;
        }

        //filter by specialties and descending price

        public async Task<List<Therapist>> FilterBySpecialtiesAndDescendingPrice(List<int> specializationIds)
        {
            var therapists = await _db.Therapists
                .Include(t => t.TherapistSpecialties)
                    .ThenInclude(ts => ts.Specialty)
                .Where(t => t.TherapistSpecialties
                    .Any(ts => specializationIds.Contains(ts.SpecialtyId)))
                .OrderByDescending(t => t.Price)
                .ToListAsync();

            return therapists;
        }

        //get specialties of a therapist
        public async Task<List<Specialty>> GetSpecialties(int therapistId)
        {
            var specialties =  await _db.TherapistSpecialties
                 .Where(ts => ts.TherapistId == therapistId)
                 .Select(ts => ts.Specialty)
                 .ToListAsync();
            return specialties;

        }

        public async Task AddPatientToTherapist(int patientId, int therapistId)
        {
            var patientTherapist = await _db.PatientTherapists.FirstOrDefaultAsync(x => x.PatientId == patientId && x.TherapistId == therapistId);
            if (patientTherapist == null)
            {
                var patientTherapist1 = new PatientTherapist
                {
                    PatientId = patientId,
                    TherapistId = therapistId
                };
                await _db.PatientTherapists.AddAsync(patientTherapist1);
                await _db.SaveChangesAsync();
            }
        }

    }
}
