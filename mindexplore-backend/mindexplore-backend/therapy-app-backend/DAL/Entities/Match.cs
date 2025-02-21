using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace therapy_app_backend.DAL.Entities
{
    public class Match
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // added primary key field
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int TherapistId { get; set; }
        public Therapist Therapist { get; set; }
        public int Score { get; set; } // added score field

    }
}
