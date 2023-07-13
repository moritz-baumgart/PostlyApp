using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using PostlyApp.Views;
using PostlyApp.Services;
using PostlyApp.Models.DTOs;

namespace PostlyApp.Pages;

public partial class HomePage : ContentPage
{
    private readonly IContentService _content;

    public HomePage()
    {
        InitializeComponent();
        _content = DependencyService.Resolve<IContentService>();
        _content.OnNewPostCreated += OnNewPostCreated;
    }

    private async void OnNewPostCreated(int postId)
    {
        var newPost = await _content.GetPost(postId);
        if (newPost != null)
        {
            var posts = new List<PostDTO>(publicFeed.Posts);
            posts.Insert(0, newPost);
            publicFeed.Posts = posts;
        }
        else
        {
            var toast = Toast.Make("Could not fetch new post!");
            await toast.Show();
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        publicFeed.Posts = await _content.GetPublicFeed(null);
        loadMorePublicBtn.IsVisible = true;
    }

    private async void OnLoadMorePublic(object sender, EventArgs e)
    {
        loadMorePublicBtn.IsEnabled = false;
        var lastPost = publicFeed.Posts.LastOrDefault();
        if (lastPost != null)
        {
            var newPosts = await _content.GetPublicFeed(lastPost.CreatedAt);
            if (newPosts == null)
            {
                var toast = Toast.Make("Error while loading more posts!");
                await toast.Show();
                return;
            }
            if (newPosts.Count == 0)
            {
                var toast = Toast.Make("No more posts to load!");
                await toast.Show();
                return;
            }
            newPosts.InsertRange(0, publicFeed.Posts);
            publicFeed.Posts = newPosts;
        }
        loadMorePublicBtn.IsEnabled = true;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        this.ShowPopup(new NewPostPopup());
    }

    
}