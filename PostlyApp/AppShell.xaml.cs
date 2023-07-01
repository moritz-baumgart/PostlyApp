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

    private void OnTokenChange(JwtSecurityToken token)
    {
        helloLabel.Text = "Hello, @" + _jwt.GetUserName() + "!";
    }

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

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        _account.Logout();
    }
}
