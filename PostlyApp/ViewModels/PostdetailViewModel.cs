
using CommunityToolkit.Mvvm.ComponentModel;
using PostlyApp.Models.DTOs;

namespace PostlyApp.ViewModels
{
    /// <summary>
    /// The view mopdel of the <see cref="PostlyApp.Views.PostdetailView"/>.
    /// </summary>
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
