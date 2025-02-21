using therapy_app_backend.DAL.Entities;

namespace therapy_app_backend.DAL.Interfaces
{
    public interface INoteRepository
    {
        Task AddNote(Note note);
        Task UpdateNote(Note note);
        Task DeleteNote(Note note);
        Task<Note> GetNoteById(int noteId);
        Task<List<Note>> GetHappyNote(int id);
        Task<List<Note>> GetNeutralNote(int id);
        Task<List<Note>> GetSadNote(int id);
        Task<List<Note>> GetNotesByPatientId(int noteId);
        Task<List<Note>> GetAllNotes(int patientId);

    }
}
