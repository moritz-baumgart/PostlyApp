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

    /// <summary>
    /// This method listens to the new post event of the ContentService.
    /// Is the user creates a new post it fetches and displays it.
    /// </summary>
    /// <param name="postId">The id of the post to fetch</param>
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

    /// <summary>
    /// Lifecycle method called when the page appears.
    /// It fetches the public ("recommended") feed and the private ("following") feed and passes it the the respective views.
    /// </summary>
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

    /// <summary>
    /// This is called when the load more btn on the public feed is clicked.
    /// It fetches more posts and adds them to the view, if there are no new posts it dispalys an info message.
    /// </summary>

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

    /// <summary>
    /// Same as <see cref="HomePage.OnLoadMorePublic(object, EventArgs)"/>, but for the private feed.
    /// </summary>
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

    /// <summary>
    /// This is called when the new post btn is clicked, it opens up the popup window with the new post UI.
    /// </summary>
    private void NewPostClicked(object sender, EventArgs e)
    {
        this.ShowPopup(new NewPostPopup());
    }

    /// <summary>
    /// This is called by <see cref="HomePage.RecBtnClicked(object, EventArgs)"/> and <see cref="HomePage.FollowBtnClicked(object, EventArgs)"/>.
    /// It sets the current tab of the tab navigation inside the view model.
    /// </summary>
    /// <param name="tabNr">The tabs nr to switch to.</param>
    private void ChangeTab(int tabNr)
    {
        if (BindingContext is HomePageViewModel viewModel)
        {
            viewModel.CurrentTab = tabNr;
        }
    }

    /// <summary>
    /// This is called when the recommended feed btn is clicked, switches tabs accordingly.
    /// </summary>
    private void RecBtnClicked(object sender, EventArgs e)
    {
        ChangeTab(0);
    }

    /// <summary>
    /// This is called when the following feed btn is clicked, switches tabs accordingly.
    /// </summary>
    private void FollowBtnClicked(object sender, EventArgs e)
    {
        ChangeTab(1);
    }
}