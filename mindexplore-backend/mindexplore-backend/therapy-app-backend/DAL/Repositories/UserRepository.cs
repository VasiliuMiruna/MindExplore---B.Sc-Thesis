using therapy_app_backend.DAL.Interfaces;
using therapy_app_backend.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace therapy_app_backend.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        
        private readonly AppDbContext _db;
        public UserRepository (AppDbContext db)
        {
         
            _db = db;
        }
        public async Task<UserRetrieve> GetUserDetails(string userId)
        {
            var p = await _db.Patients.FirstOrDefaultAsync(p =>  p.User.Id == userId); 
            var t = await _db.Therapists.FirstOrDefaultAsync(t => t.User.Id == userId);
            string firstName;
            string lastName;

            var a = await _db.Users.FirstOrDefaultAsync(a => a.Id == userId);
          
            
           if (p != null)
            {
                firstName = p.FirstName;
                lastName = p.LastName;
            }

            else if(t != null)
            {
                firstName = t.FirstName;
                lastName = t.LastName;
            }
            else 
            {
                firstName = "admin";
                lastName = "admin";
            }

            var userModel = new UserRetrieve
            {
                FirstName = firstName,
                LastName = lastName,

            };
            return userModel;

        }
        public int GetShortIdByUserId(string userId, string role)
        {
            int id = 0;
            if (role == "Patient")
            {
                var patient = _db.Patients.FirstOrDefault(p => p.UserId == userId);
                if (patient != null)
                    id = patient.PatientId;
                
            }
            else if (role == "Therapist")
            {
                var therapist = _db.Therapists.FirstOrDefault(p => p.UserId == userId);
                if (therapist != null)
                    id = therapist.TherapistId;
                
            }
            return id;
        }
        public async Task<string> GetUserIdByShortId(int id, string role)
        {
            string userId = "";
            if (role == "Patient")
            {
                var patient = await _db.Patients.FirstOrDefaultAsync(p => p.PatientId == id);
                if (patient != null)
                    userId = patient.UserId;

            }
            else if (role == "Therapist")
            {
                var therapist =await  _db.Therapists.FirstOrDefaultAsync(p => p.TherapistId == id);
                if (therapist != null)
                    userId = therapist.UserId;

            }
            return userId;
        }

    }
}
