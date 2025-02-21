using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace therapy_app_backend.DAL.Entities
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required] public string Sender { get; set; }

        [Required] public string Receiver { get; set; }

        [Required] public string ConnectionId { get; set; }
        [Required] public string RoomName { get; set; }

        [Required] public string Text { get; set; }
        public byte[]? ImageData { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}