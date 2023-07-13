using CommunityToolkit.Mvvm.ComponentModel;
using PostlyApp.Enums;

namespace PostlyApp.Models.DTOs
{
    public partial class PostDTO : ObservableObject
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public UserDTO Author { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string? AttachedImageUrl { get; set; }
        [ObservableProperty]
        private int _upvoteCount;
        [ObservableProperty]
        private int _downvoteCount;
        [ObservableProperty]
        private int _commentCount;
        [ObservableProperty]
        private VoteType? _vote;
        [ObservableProperty]
        private bool? _hasCommented;
    }
}
