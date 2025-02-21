using System.ComponentModel.DataAnnotations;

namespace therapy_app_backend.BLL.Models
{
    public class TherapistRegisterModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string LicenseNumber { get; set; }

        [Required]
        public string LicenseState { get; set; }


    }
}
