using System.ComponentModel.DataAnnotations;

namespace therapy_app_backend.BLL.Models
{
    public class MessageModel
    {
        public int Id { get; set; }

        [Required]
        public string User { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;


    }
}
