using PostlyApp.Models.DTOs;

namespace PostlyApp.Services
{
    public interface IAccountService
    {
        public Task<bool> GetStatus();
        public Task<bool?> Login(string username, string password);
        public Task<bool?> Register(string username, string password);
        public void Logout();
        public Task<UserProfileViewModel?> GetUserProfile(string? username);
        public Task<UserProfileViewModel?> FollowUser(string username);
        public Task<UserProfileViewModel?> UnfollowUser(string username);
    }
}
