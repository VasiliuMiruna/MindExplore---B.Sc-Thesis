using therapy_app_backend.DAL.Entities;

namespace therapy_app_backend.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<UserRetrieve> GetUserDetails(string userId);
        int GetShortIdByUserId(string userId, string role);
        Task<string> GetUserIdByShortId(int id, string role);
    }
}
