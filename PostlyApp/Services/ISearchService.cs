using PostlyApp.Models.DTOs;

namespace PostlyApp.Services
{
    interface ISearchService
    {
        public Task<IEnumerable<UserDTO>?> SearchUsers(string username);

    }
}
