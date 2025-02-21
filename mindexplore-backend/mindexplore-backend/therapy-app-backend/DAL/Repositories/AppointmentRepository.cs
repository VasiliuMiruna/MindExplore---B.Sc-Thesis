using Microsoft.EntityFrameworkCore;
using therapy_app_backend.DAL.Entities;
using therapy_app_backend.DAL.Interfaces;

namespace therapy_app_backend.DAL.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _db;

        public AppointmentRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task CreateAppointment(Appointment appointment)
        {
            _db.Appointments.Add(appointment);
            await _db.SaveChangesAsync();
        }
        public async Task UpdateAppointment(Appointment appointment)
        {
            _db.Appointments.Update(appointment);
            await _db.SaveChangesAsync();
        }
        public async Task<Appointment> GetAppointment(int appointmentId)
        {
            var appointment = await _db.Appointments.FirstOrDefaultAsync(a => a.Id == appointmentId);
            return appointment;
        }
        public async Task<List<Appointment>> GetAppointmentsByTherapistId(int therapistId)
        {
            var appointments = await _db.Appointments
                .Where(p => p.TherapistId == therapistId)
                .Include(a => a.Patient)
                .ToListAsync();
            return appointments;
        }
        public async Task<List<Appointment>> GetAppointmentsByPatientId(int patientId)
        {
            var appointments = await _db.Appointments
                .Where(p => p.PatientId == patientId)
                .Include(a => a.Therapist)
                .ToListAsync();
            return appointments;
        }
       
       public async Task DeleteAppointment(Appointment appointment)
       {
            _db.Appointments.Remove(appointment);
            await _db.SaveChangesAsync();
       }
    }
}
