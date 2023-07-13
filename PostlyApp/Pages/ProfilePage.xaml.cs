using CommunityToolkit.Maui.Views;
using MauiToolkitPopupSample;
using PostlyApp.Models.DTOs;
using PostlyApp.Services;

namespace PostlyApp.Pages;

[QueryProperty(nameof(Username), nameof(Username))]
public partial class ProfilePage : ContentPage
{

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

    UserProfileViewModel? userProfile;
    public UserProfileViewModel? UserProfile
    {
        get => userProfile;
        set
        {
            userProfile = value;
            OnPropertyChanged();
        }
    }

    private readonly IJwtService _jwt;
    private readonly IAccountService _account;

    public ProfilePage()
    {
        InitializeComponent();
        BindingContext = this;
        _jwt = DependencyService.Resolve<IJwtService>();
        _account = DependencyService.Resolve<IAccountService>();
    }

    protected override async void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        base.OnNavigatingFrom(args);

        // We use this lifecycle besides OnAppearing because it is always called, even when we are already on this page and click it again
        // This way the user can go back to their profile by clicking "Profile" again, even when they are on another users profile.
        Username = null;
        await FetchCurrentUser();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await FetchCurrentUser();
    }

    private async Task FetchCurrentUser()
    {
        if (username != null && !username.Equals(_jwt.GetUserName()))
        {
            UserProfile = await _account.GetUserProfile(username);
        }
        else
        {
            UserProfile = await _account.GetUserProfile(null);
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        this.ShowPopup(new PostdetailView());
    }
}