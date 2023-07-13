using PostlyApp.Enums;

namespace PostlyApp.Models.DTOs
{
    public class VoteUpdateViewModel
    {
        public int PostId { get; set; } // the post that had a vote update
        public int UpvoteCount { get; set; } // the upvote count after the vote update
        public int DownvoteCount { get; set; } // the downvote count after the vote update
        public long UserId { get; set; } // the user who updated their vote
        public VoteType? VoteType { get; set; } // the type of vote the user set
    }
}
