using System.ComponentModel.DataAnnotations;

namespace therapy_app_backend.DAL.Entities
{
    public class Conversation
    {
        [Key]
        public Guid ConversationId { get; set; }
        public string PatientId { get; set; }
        public string TherapistId { get; set; }

    }
}
