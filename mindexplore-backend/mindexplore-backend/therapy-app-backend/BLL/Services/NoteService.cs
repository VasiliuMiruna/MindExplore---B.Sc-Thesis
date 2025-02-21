using therapy_app_backend.BLL.Interfaces;
using therapy_app_backend.BLL.Models;
using therapy_app_backend.DAL.Entities;
using therapy_app_backend.DAL.Interfaces;

namespace therapy_app_backend.BLL.Services
{
    public class NoteService : INoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        public NoteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task CreateNoteForPatient(int id, NoteModel note)
        {
            var patient = await _unitOfWork.Patients.GetById(id);
            if(patient == null)
            {
                throw new Exception("Patient not found!");
            }
            else
            {
                Note noteDb = new Note
                {
                    
                    CreatedDate = DateTime.Now,
                    Content = note.Content,
                    Feeling = note.Feeling
                };
                noteDb.PatientId = id;
                await _unitOfWork.Notes.AddNote(noteDb);
            }
        }
        public async Task<List<NoteModel>> GetAllNotes(int id)
        {
            var notes = await _unitOfWork.Notes.GetNotesByPatientId(id);
            var notesModel = new List<NoteModel>();
            foreach (var note in notes)
            {
                var noteM = new NoteModel
                {
                    Id = note.Id,
                    CreatedDate = note.CreatedDate,
                    Content = note.Content,
                    Feeling = note.Feeling
                };
                notesModel.Add(noteM);

            }
            return notesModel;
        }
        ///     TODO
        public async Task<FeelingModel> GetFeelingStatistic(int id)
        {
            var happyNotes = await _unitOfWork.Notes.GetHappyNote(id);
            var neutralNotes = await _unitOfWork.Notes.GetNeutralNote(id);
            var sadNotes = await _unitOfWork.Notes.GetSadNote(id);
            var feelingModel = new FeelingModel
            {
                Happy = happyNotes.Count(),
                Neutral = neutralNotes.Count(),
                Sad = sadNotes.Count(),
            };
            return  feelingModel;
        }
        public async Task UpdateNote(int id, NoteModel note)
        {
            var newNote = await _unitOfWork.Notes.GetNoteById(id);
            newNote.Content = note.Content;
            newNote.CreatedDate = DateTime.Now;
            await _unitOfWork.Notes.UpdateNote(newNote);

        }
        public async Task DeleteNote(int id)
        {
            var note = await _unitOfWork.Notes.GetNoteById(id);
            if (note != null)
            {
                await _unitOfWork.Notes.DeleteNote(note);
            }
                
        }

    }
}
