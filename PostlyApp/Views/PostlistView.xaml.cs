using PostlyApp.Models.DTOs;

namespace PostlyApp.Views;

public partial class PostlistView : ContentView
{

    public static readonly BindableProperty PostsProperty = BindableProperty.Create(nameof(Posts), typeof(List<PostDTO>), typeof(PostlistView), propertyChanged: OnPostsChanged);
    public List<PostDTO> Posts
    {
        get => (List<PostDTO>)GetValue(PostsProperty);
        set => SetValue(PostsProperty, value);
    }

    public PostlistView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Called when the list of posts changes, updates the child element that display the posts.
    /// </summary>
    private static void OnPostsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PostlistView view)
        {
            view.postContainer.Children.Clear();

            if (view.Posts != null)
            {
                foreach (var post in view.Posts)
                {
                    var postView = new PostView(post);
                    view.postContainer.Add(postView);
                }
            }
        }

    }
}