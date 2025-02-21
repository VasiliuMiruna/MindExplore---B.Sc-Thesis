using System.ComponentModel.DataAnnotations;

namespace therapy_app_backend.DAL.Entities
{
    public class AppUserRefreshToken
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
