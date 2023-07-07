using CommunityToolkit.Maui.Alerts;
using PostlyApp.Services;

namespace PostlyApp.Pages;

public partial class HomePage : ContentPage
{
    private readonly IContentService _content;

    public HomePage()
    {
        InitializeComponent();
        _content = DependencyService.Resolve<IContentService>();
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
}