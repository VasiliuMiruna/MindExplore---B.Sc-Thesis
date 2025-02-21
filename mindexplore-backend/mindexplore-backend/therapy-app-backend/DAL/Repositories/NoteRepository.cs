using Microsoft.EntityFrameworkCore;
using therapy_app_backend.DAL.Entities;
using therapy_app_backend.DAL.Interfaces;

namespace therapy_app_backend.DAL.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly AppDbContext _db;
        public NoteRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddNote(Note note)
        {
            _db.Notes.Add(note);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateNote(Note note)
        {
            _db.Notes.Update(note);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteNote(Note note)
        {
            _db.Notes.Remove(note);
            await _db.SaveChangesAsync();
        }
        public async Task<Note> GetNoteById(int noteId)
        {
            var note = await _db.Notes.FirstOrDefaultAsync(s => s.Id == noteId);
            return note;
        }

        public async Task<List<Note>> GetHappyNote(int id)
        {
            var today = DateTime.Now.Date;
            var sevenDaysAgo = today.AddDays(-7);
            var notes = await _db.Notes
                            .Where(n => n.Feeling == "happy" && n.PatientId == id && n.CreatedDate >= sevenDaysAgo && n.CreatedDate <= today)
                            .ToListAsync();
            return notes;
        }

        public async Task<List<Note>> GetNeutralNote(int id)
        {
            var today = DateTime.Now.Date;
            var sevenDaysAgo = today.AddDays(-7);
            var notes = await _db.Notes
                            .Where(n => n.Feeling == "neutral" && n.PatientId == id && n.CreatedDate >= sevenDaysAgo && n.CreatedDate <= today)
                            .ToListAsync();
            return notes;
        }

        public async Task<List<Note>> GetSadNote(int id)
        {
            var today = DateTime.Now.Date;
            var sevenDaysAgo = today.AddDays(-7);
            var notes = await _db.Notes
                            .Where(n => n.Feeling == "sad" && n.PatientId == id && n.CreatedDate >= sevenDaysAgo && n.CreatedDate <= today)
                            .ToListAsync();
            return notes;
        }
        public async Task<List<Note>> GetNotesByPatientId(int patientId)
        {
            var notes = await _db.Notes
                .Where(s => s.PatientId == patientId)
                .ToListAsync();
            return notes;
        }
        public async Task<List<Note>> GetAllNotes(int patientId)
        {
            var notes = await _db.Notes
                .Where(s => s.PatientId == patientId)
                .ToListAsync();
            return notes;
        }

    }
}
