using PostlyApp.Models.DTOs;

namespace PostlyApp.Services
{
    internal interface IContentService
    {
        public Task<List<PostDTO>> GetPublicFeed(DateTimeOffset? paginationStart);
    }
}
