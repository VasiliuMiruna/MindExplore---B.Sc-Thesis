namespace therapy_app_backend.BLL.Models
{
    public class NoteModel
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Content { get; set; }
        public string Feeling { get; set; }
    }
}
