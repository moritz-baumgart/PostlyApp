using PostlyApp.Services;
using PostlyApp.Services.Impl;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;

namespace PostlyApp;

public partial class AppShell : Shell, INotifyPropertyChanged
{
    private IAccountService _account;
    private IJwtService _jwt;

    public AppShell()
    {
        InitializeComponent();

        _account = DependencyService.Resolve<IAccountService>();
        _jwt = DependencyService.Resolve<IJwtService>(); ;

        _jwt.CurrentTokenChanged += OnTokenChange;
    }

    /// <summary>
    /// Is called when the jwt token changes, updates the hello message inside the app shells flyout.
    /// </summary>
    /// <param name="token">The new token.</param>
    private void OnTokenChange(JwtSecurityToken token)
    {
        helloLabel.Text = "Hello, @" + _jwt.GetUserName() + "!";
    }

    /// <summary>
    /// Called on startup, redirects the user to the login page, if they are not logged in, otherwise updates the hello message in the flyout.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Depending on login status show home page or login page and enable/disable flyout menu
        var status = await _account.GetStatus();
        if (status)
        {
            PostlyAppShell.CurrentItem = PostlyHomePage;

            // If the user is logged in we can load their username into the flyout ui:
            helloLabel.Text = "Hello, @" + _jwt.GetUserName() + "!";
        }
        else
        {
            PostlyAppShell.CurrentItem = PostlyLoginPage;
        }
    }

    /// <summary>
    /// Logs the user out, when the logout btn is clicked.
    /// </summary>
    private void OnLogoutClicked(object sender, EventArgs e)
    {
        _account.Logout();
    }
}
