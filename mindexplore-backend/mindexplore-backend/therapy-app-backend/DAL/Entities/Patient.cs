using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace therapy_app_backend.DAL.Entities
{
    
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PatientId {get;set;}

        public string? LastName {get;set;}
        public string? FirstName { get;set;}
        public int? Age { get; set;}

        public string? City { get; set;}
        //public List<string>? Preferences { get; set;}

        public virtual ICollection<Match>? Matches { get; set; }
        public virtual ICollection<Appointment>? Appointments { get; set; }
        public virtual ICollection<PatientTherapist>? PatientTherapists { get; set; }
        public virtual ICollection<Note>? Notes { get; set; }

        public virtual string UserId { get; set; }
        public virtual AppUser User { get; set; }




    }
}
