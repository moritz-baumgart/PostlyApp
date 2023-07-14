using CommunityToolkit.Mvvm.ComponentModel;
using PostlyApp.Enums;
using PostlyApp.Models.DTOs;
using PostlyApp.Utilities;
using System.ComponentModel;

namespace PostlyApp.ViewModels
{
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

        private void OnVoteChange(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PostDTO.Vote))
            {
                OnVoteChange(Post.Vote);
            }
        }

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
