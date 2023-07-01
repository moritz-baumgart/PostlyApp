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
            postView.content.Text = postView.Post.Content;
            postView.author.Text = postView.Post.Author.Username;
        }
    }

    public PostView()
    {
        InitializeComponent();
    }
}