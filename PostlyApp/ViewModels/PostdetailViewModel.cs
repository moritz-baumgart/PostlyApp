
using CommunityToolkit.Mvvm.ComponentModel;
using PostlyApp.Models.DTOs;

namespace PostlyApp.ViewModels
{
    internal partial class PostdetailViewModel : ObservableObject
    {
        [ObservableProperty]

        private List<CommentDTO> _comments;
        [ObservableProperty]
        private PostDTO _post;

        public PostdetailViewModel(PostDTO post)
        {
            Post = post;
        }
    }
}
