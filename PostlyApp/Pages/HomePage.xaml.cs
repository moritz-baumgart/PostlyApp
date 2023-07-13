using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using PostlyApp.Views;
using PostlyApp.Services;
using PostlyApp.Models.DTOs;
using PostlyApp.ViewModels;

namespace PostlyApp.Pages;

public partial class HomePage : ContentPage
{
    private readonly IContentService _content;

    public HomePage()
    {
        InitializeComponent();
        _content = DependencyService.Resolve<IContentService>();
        _content.OnNewPostCreated += OnNewPostCreated;
        BindingContext = new HomePageViewModel();
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

        var pubFeedTask = _content.GetPublicFeed(null);
        var privFeedTask = _content.GetPrivateFeed(null);

        publicFeed.Posts = await pubFeedTask;
        loadMorePublicBtn.IsVisible = true;

        privateFeed.Posts = await privFeedTask;
        loadMorePrivateBtn.IsVisible = true;
    }

    private async void OnLoadMorePublic(object sender, EventArgs e)
    {
        loadMorePublicBtn.IsEnabled = false;
        var lastPost = publicFeed.Posts.LastOrDefault();
        List<PostDTO>? newPosts;
        if (lastPost != null)
        {
            newPosts = await _content.GetPublicFeed(lastPost.CreatedAt);
        }
        else
        {
            newPosts = await _content.GetPublicFeed(null);
        }
        if (newPosts == null)
        {
            var toast = Toast.Make("Error while loading more posts!");
            await toast.Show();
            loadMorePublicBtn.IsEnabled = true;
            return;
        }
        if (newPosts.Count == 0)
        {
            var toast = Toast.Make("No more posts to load!");
            await toast.Show();
            loadMorePublicBtn.IsEnabled = true;
            return;
        }
        newPosts.InsertRange(0, publicFeed.Posts);
        publicFeed.Posts = newPosts;
        loadMorePublicBtn.IsEnabled = true;
    }

    private async void OnLoadMorePrivate(object sender, EventArgs e)
    {
        loadMorePrivateBtn.IsEnabled = false;
        var lastPost = privateFeed.Posts.LastOrDefault();
        List<PostDTO>? newPosts;
        if (lastPost != null)
        {
            newPosts = await _content.GetPrivateFeed(lastPost.CreatedAt);
        }
        else
        {
            newPosts = await _content.GetPrivateFeed(null);
        }

        if (newPosts == null)
        {
            var toast = Toast.Make("Error while loading more posts!");
            await toast.Show();
            loadMorePrivateBtn.IsEnabled = true;
            return;
        }
        if (newPosts.Count == 0)
        {
            var toast = Toast.Make("No more posts to load!");
            await toast.Show();
            loadMorePrivateBtn.IsEnabled = true;
            return;
        }
        newPosts.InsertRange(0, privateFeed.Posts);
        privateFeed.Posts = newPosts;

        loadMorePrivateBtn.IsEnabled = true;
    }

    private void NewPostClicked(object sender, EventArgs e)
    {
        this.ShowPopup(new NewPostPopup());
    }

    private void ChangeTab(int tabNr)
    {
        if (BindingContext is HomePageViewModel viewModel)
        {
            viewModel.CurrentTab = tabNr;
        }
    }

    private void RecBtnClicked(object sender, EventArgs e)
    {
        ChangeTab(0);
    }

    private void FollowBtnClicked(object sender, EventArgs e)
    {
        ChangeTab(1);
    }
}