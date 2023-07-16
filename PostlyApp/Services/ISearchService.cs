using PostlyApp.Models.DTOs;

namespace PostlyApp.Services
{
    interface ISearchService
    {
        /// <summary>
        /// Searches for users and returns the search result.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="UserDTO"/> if successful, null otherwise.</returns>
        public Task<IEnumerable<UserDTO>?> SearchUsers(string username);

    }
}
