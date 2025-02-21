using therapy_app_backend.BLL.Models;

namespace therapy_app_backend.BLL.Interfaces
{
    public interface INoteService
    {
        Task CreateNoteForPatient(int id, NoteModel note);
        Task<List<NoteModel>> GetAllNotes(int id);
        Task UpdateNote(int id, NoteModel note);
        Task DeleteNote(int id);
        Task<FeelingModel> GetFeelingStatistic(int id);

    }
}
