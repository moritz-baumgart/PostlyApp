using PostlyApp.Models.DTOs;

namespace PostlyApp.Services
{
    public interface IAccountService
    {
        /// <summary>
        /// Retrieves the current login state.
        /// </summary>
        /// <returns>False if no jwt is set or the user is not authenticated, true otherwise.</returns>
        public Task<bool> GetStatus();

        /// <summary>
        /// Fetches the users that follow the specified user.
        /// </summary>
        /// <param name="username">The username of the user to fetch the followers for.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="UserDTO"/> if the request was successful, null otherwise.</returns>
        public Task<List<UserDTO>?> GetFollowers(string? username);

        /// <summary>
        /// Fetches the users the specified user follows.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="UserDTO"/> if the request was successful, null otherwise.</returns>
        public Task<List<UserDTO>?> GetFollowing(string? username);

        /// <summary>
        /// Logs in a user with given credentials. Also saves jwt using <see cref="JwtService"/>.
        /// </summary>
        /// <returns>True if login was successful, false if username/password was wrong, null if something went wrong.</returns>
        public Task<bool?> Login(string username, string password);

        /// <summary>
        /// Tries to registers a user with the given credentials.
        /// </summary>
        /// <returns>Returns true if successful, false if the username is already in use, null if something went wrong.</returns>
        public Task<bool?> Register(string username, string password);

        /// <summary>
        /// Logs the user out by deleting the JWT and navigating back to the login.
        /// </summary>
        public void Logout();

        /// <summary>
        /// Fetches the profile details of a users.
        /// </summary>
        /// <param name="username">The username of the user to fetch the details for.</param>
        /// <returns>An <see cref="UserProfileViewModel"/> if the request was successful, null otherwise.</returns>
        public Task<UserProfileViewModel?> GetUserProfile(string? username);

        /// <summary>
        /// Follows the given user.
        /// </summary>
        /// <returns>An updated <see cref="UserProfileViewModel"/> if the request was successful, null otherwise.</returns>
        public Task<UserProfileViewModel?> FollowUser(string username);

        /// <summary>
        /// Unfollows the given user.
        /// </summary>
        /// <returns>An updated <see cref="UserProfileViewModel"/> if the request was successful, null otherwise.</returns>
        public Task<UserProfileViewModel?> UnfollowUser(string username);
    }
}
