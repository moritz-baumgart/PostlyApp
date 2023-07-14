using CommunityToolkit.Mvvm.ComponentModel;

namespace PostlyApp.Models.DTOs
{
    public partial class CommentDTO : ObservableObject
    {
        int Id { get; set; }
        public UserDTO Author { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        [ObservableProperty]
        private string _content;
    }
}
