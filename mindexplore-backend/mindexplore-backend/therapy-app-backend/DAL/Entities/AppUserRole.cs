using Microsoft.AspNetCore.Identity;

namespace therapy_app_backend.DAL.Entities
{
    public class AppUserRole : IdentityUserRole <string>
    {
        public virtual AppUser User { get; set; }
    }
}
