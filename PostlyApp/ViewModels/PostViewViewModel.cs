using CommunityToolkit.Mvvm.ComponentModel;
using PostlyApp.Enums;
using PostlyApp.Models.DTOs;
using PostlyApp.Utilities;
using System.ComponentModel;

namespace PostlyApp.ViewModels
{
    /// <summary>
    /// The view model of the <see cref="PostlyApp.Views.PostView"/>.
    /// </summary>
    internal partial class PostViewViewModel : ObservableObject
    {

        [ObservableProperty]
        private PostDTO _post;
        [ObservableProperty]
        private UriImageSource? _postImgUrl;
        [ObservableProperty]
        private ImageSource _upvoteImg;
        [ObservableProperty]
        private ImageSource _downvoteImg;



        public PostViewViewModel(PostDTO post)
        {
            Post = post;
            post.PropertyChanged += OnVoteChange;
            OnVoteChange(post.Vote);
        }

        /// <summary>
        /// Updates the AttachedImageUrl property when the post changes. This is required because the img url needs some additional formatting/type conversion.
        /// </summary>
        /// <param name="newPost">The new post.</param>
        partial void OnPostChanged(PostDTO newPost)
        {
            if (newPost.AttachedImageUrl != null)
            {
                PostImgUrl = new UriImageSource
                {
                    Uri = new Uri(Constants.API_BASE + newPost.AttachedImageUrl)
                };
            }
            else
            {
                PostImgUrl = null;
            }
        }

        /// <summary>
        /// Updates the <see cref="UpvoteImg"/> and <see cref="DownvoteImg"/> with the help of <see cref="OnVoteChange"/> everytime the vote property changes.
        /// </summary>
        private void OnVoteChange(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PostDTO.Vote))
            {
                OnVoteChange(Post.Vote);
            }
        }

        /// <summary>
        /// Used by <see cref="OnVoteChange(object?, PropertyChangedEventArgs)"/> and at initialization to set the image reflecting the value of the vote on this post.
        /// </summary>
        /// <param name="vote">The vote type to react to.</param>
        private void OnVoteChange(VoteType? vote)
        {
            if (vote != null)
            {
                if (vote == VoteType.Upvote)
                {
                    UpvoteImg = "thumbs_up_solid.png";
                    DownvoteImg = "thumbs_down_regular.png";
                }
                else if (vote == VoteType.Downvote)
                {
                    UpvoteImg = "thumbs_up_regular.png";
                    DownvoteImg = "thumbs_down_solid.png";
                }
            }
            else
            {
                UpvoteImg = "thumbs_up_regular.png";
                DownvoteImg = "thumbs_down_regular.png";
            }
        }
    }
}
