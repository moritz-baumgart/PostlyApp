using PostlyApp.Enums;
using PostlyApp.Models.DTOs;

namespace PostlyApp.Services
{
    internal interface IContentService
    {
        /// <summary>
        /// Fetches the public feed.
        /// </summary>
        /// <param name="paginationStart">The start of the feed, all fetched post will be older than this.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="PostDTO"/> if successful, null otherwise.</returns>
        public Task<List<PostDTO>?> GetPublicFeed(DateTimeOffset? paginationStart);

        /// <summary>
        /// Fetches the private feed.
        /// </summary>
        /// <param name="paginationStart">The start of the feed, all fetched post will be older than this.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="PostDTO"/> if successful, null otherwise.</returns>
        public Task<List<PostDTO>?> GetPrivateFeed(DateTimeOffset? paginationStart);

        /// <summary>
        /// Fetches the posts of a single user.
        /// </summary>
        /// <param name="username">The username of the user to fetch the posts for.</param>
        /// <param name="paginationStart">The start of the feed, all fetched post will be older than this.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="PostDTO"/> if successful, null otherwise.</returns>
        public Task<List<PostDTO>?> GetProfileFeed(string? username, DateTimeOffset? paginationStart);

        /// <summary>
        /// Fetches the comments of a post.
        /// </summary>
        /// <param name="postId"></param>
        /// <returns>A <see cref="List{T}"/> of <see cref="CommentDTO"/>s.</returns>
        public Task<List<CommentDTO>?> GetComments(long postId);

        /// <summary>
        /// Submits a vote for a post.
        /// </summary>
        /// <param name="postId">The id of the post to apply the vote on.</param>
        /// <param name="voteType">The type of vote to submit.</param>
        /// <returns>A <see cref="VoteUpdateViewModel"/> with new posts data if successful, null otehrwise.</returns>
        public Task<VoteUpdateViewModel?> SetVote(int postId, VoteType voteType);

        /// <summary>
        /// Removes a vote from a post.
        /// </summary>
        /// <param name="postId">The id of the post to remove the vote from</param>
        /// <returns>A <see cref="VoteUpdateViewModel"/> with new posts data if successful, null otehrwise.</returns>
        public Task<VoteUpdateViewModel?> RemoveVote(int postId);

        /// <summary>
        /// Tries to create a new post.
        /// </summary>
        /// <param name="content">The text content of the post.</param>
        /// <returns>The new post's id if successful, null otherwise.</returns>
        public Task<int?> AddPost(string content);

        /// <summary>
        /// Adds a comment to a post.
        /// </summary>
        /// <param name="postId">The id of the post the comment should be added to.</param>
        /// <param name="commentContent">The text content of the comment.</param>
        /// <returns>The new number of comments on that post if the request was successful, null otherwise.</returns>
        public Task<int?> AddComment(int postId, string commentContent);

        /// <summary>
        /// Fetches a post.
        /// </summary>
        /// <param name="postId">The id of the post to fetch.</param>
        /// <returns>A <see cref="PostDTO"/> if successful, null otherwise.</returns>
        public Task<PostDTO?> GetPost(long postId);

        /// <summary>
        /// An event invoked when a new post was created. Can be used to update views when the user posts something.
        /// </summary>
        public event Action<int> OnNewPostCreated;
    }
}
