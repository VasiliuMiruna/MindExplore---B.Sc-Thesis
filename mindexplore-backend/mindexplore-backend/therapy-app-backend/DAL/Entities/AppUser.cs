using Microsoft.AspNetCore.Identity;

namespace therapy_app_backend.DAL.Entities
{
    public class AppUser : IdentityUser
    {
        public DateTime TokenExpiration { get; set; }
        public List<AppUserRefreshToken>? RefreshTokens { get; set; }
        public Patient? Patient { get; set; }
        public Therapist? Therapist { get; set; }

    }
}
