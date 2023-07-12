using PostlyApp.Models.DTOs;

namespace PostlyApp.Views;

public partial class PostView : ContentView
{

    public PostDTO Post
    {
        get => (PostDTO)GetValue(PostProperty);
        set => SetValue(PostProperty, value);
    }

    public static BindableProperty PostProperty = BindableProperty.Create(nameof(Post), typeof(PostDTO), typeof(PostView), propertyChanged: OnChange);

    private static void OnChange(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PostView postView)
        {
            if (postView.Post.Author.DisplayName != null && postView.Post.Author.DisplayName.Length > 0)
            {
                postView.author.Text = postView.Post.Author.DisplayName + " (@" + postView.Post.Author.Username + ")";
            }
            else
            {
                postView.author.Text = "@" + postView.Post.Author.Username;
            }
            postView.date.Text = postView.Post.CreatedAt.ToLocalTime().ToString();
            postView.content.Text = postView.Post.Content;
            postView.UpvoteCount.Text =  "" + postView.Post.UpvoteCount.ToString();
            postView.DownvoteCount.Text = "" + postView.Post.DownvoteCount.ToString();
            postView.CommentCount.Text = "" + postView.Post.CommentCount.ToString();
        }
    }

    public PostView()
    {
        InitializeComponent();
    }
}