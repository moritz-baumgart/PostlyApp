using PostlyApp.Enums;
using PostlyApp.Models.DTOs;

namespace PostlyApp.Services
{
    internal interface IContentService
    {
        public Task<List<PostDTO>?> GetPublicFeed(DateTimeOffset? paginationStart);
        public Task<List<PostDTO>?> GetPrivateFeed(DateTimeOffset? paginationStart);
        public Task<List<PostDTO>?> GetProfileFeed(string? username, DateTimeOffset? paginationStart);
        public Task<VoteUpdateViewModel?> SetVote(int postId, VoteType voteType);
        public Task<VoteUpdateViewModel?> RemoveVote(int postId);

        public Task<int?> AddPost(string content);

        public Task<PostDTO?> GetPost(long postId);
        public event Action<int> OnNewPostCreated;
    }
}
