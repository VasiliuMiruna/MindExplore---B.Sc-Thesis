using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace therapy_app_backend.DAL.Entities
{
    public class Specialty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        //maybe later add materials

        public virtual ICollection<TherapistSpecialty>? TherapistSpecialties { get; set; }
    }
}
