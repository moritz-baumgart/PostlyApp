using CommunityToolkit.Maui.Alerts;
using PostlyApp.Models.DTOs;
using PostlyApp.Services;
using PostlyApp.ViewModels;

namespace PostlyApp.Pages;

[QueryProperty(nameof(Username), nameof(Username))]
public partial class ProfilePage : ContentPage
{

    /// <summary>
    /// Currently displayed User's username. Is set as navigation query parameter.
    /// </summary>
    string? username;
    public string? Username
    {
        get => username;
        set
        {
            username = value;
            OnPropertyChanged();
        }
    }

    private readonly IJwtService _jwt;
    private readonly IAccountService _account;
    private readonly IContentService _content;

    public ProfilePage()
    {
        InitializeComponent();
        BindingContext = new ProfilePageViewModel();
        _jwt = DependencyService.Resolve<IJwtService>();
        _account = DependencyService.Resolve<IAccountService>();
        _content = DependencyService.Resolve<IContentService>();
    }

    /// <summary>
    /// Lifecylce method that calls <see cref="ProfilePage.FetchCurrentUser"/> to load the users profile.
    /// </summary>
    protected override async void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        base.OnNavigatingFrom(args);

        // We use this lifecycle besides OnAppearing because it is always called, even when we are already on this page and click it again
        // This way the user can go back to their profile by clicking "Profile" again, even when they are on another users profile.
        Username = null;
        await FetchCurrentUser();
    }

    /// <summary>
    /// Lifecylce method that calls <see cref="ProfilePage.FetchCurrentUser"/> to load the users profile.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await FetchCurrentUser();
    }

    /// <summary>
    /// Fetches the profile of the with the username of the <see cref="ProfilePage.username"/> property,
    /// which is set when navigating to this page, if it is not set it fetches it for the currently logged in user.
    /// </summary>
    /// <returns></returns>
    private async Task FetchCurrentUser()
    {
        if (BindingContext is ProfilePageViewModel viewModel)
        {

            if (username != null && !username.Equals(_jwt.GetUserName()))
            {
                viewModel.UserProfile = await _account.GetUserProfile(username);
                viewModel.FollowBtnVisible = true;
            }
            else
            {
                viewModel.UserProfile = await _account.GetUserProfile(null);
                viewModel.FollowBtnVisible = false;
            }
            profileFeed.Posts = await _content.GetProfileFeed(username, null);
            loadMoreBtnProfile.IsVisible = true;
        }
    }

    /// <summary>
    /// This is called when the load more posts btn on the profile is called.
    /// Fetches more posts and dispalys them, if there are no more posts it displays an info message.
    /// </summary>
    private async void OnLoadMoreProfile(object sender, EventArgs e)
    {
        loadMoreBtnProfile.IsEnabled = false;
        var lastPost = profileFeed.Posts.LastOrDefault();
        List<PostDTO>? newPosts;
        if (lastPost != null)
        {
            newPosts = await _content.GetProfileFeed(username, lastPost.CreatedAt);
        }
        else
        {
            newPosts = await _content.GetProfileFeed(username, null);
        }

        if (newPosts == null)
        {
            var toast = Toast.Make("Error while loading more posts!");
            await toast.Show();
            loadMoreBtnProfile.IsEnabled = true;
            return;
        }
        if (newPosts.Count == 0)
        {
            var toast = Toast.Make("No more posts to load!");
            await toast.Show();
            loadMoreBtnProfile.IsEnabled = true;
            return;
        }
        newPosts.InsertRange(0, profileFeed.Posts);
        profileFeed.Posts = newPosts;
        loadMoreBtnProfile.IsEnabled = true;
    }

    /// <summary>
    /// This method is called when the follow btn is clicked, depending on the view models follow state it follows or unfollows the displayed user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void FollowBtnClicked(object sender, EventArgs e)
    {
        followBtn.IsEnabled = false;

        if (BindingContext is ProfilePageViewModel viewModel)
        {
            var follow = viewModel.UserProfile?.Follow;
            if (viewModel.UserProfile != null && follow != null)
            {
                UserProfileViewModel? newProfile;
                if ((bool)follow)
                {
                    newProfile = await _account.UnfollowUser(viewModel.UserProfile.Username);
                }
                else
                {
                    newProfile = await _account.FollowUser(viewModel.UserProfile.Username);
                }

                if (newProfile != null)
                {
                    viewModel.UserProfile = newProfile;
                }
                else
                {
                    var toast = Toast.Make("Error submitting your follow/unfollow");
                    await toast.Show();
                }
            }
        }

        followBtn.IsEnabled = true;
    }
}