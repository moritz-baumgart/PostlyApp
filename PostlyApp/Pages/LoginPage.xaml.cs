using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using PostlyApp.Services;

namespace PostlyApp.Pages;

public partial class LoginPage : ContentPage
{
    private readonly IAccountService _account;

    public LoginPage()
    {
        InitializeComponent();
        _account = DependencyService.Resolve<IAccountService>();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        LoginBtn.IsEnabled = false;

        if (UsernameEntry.Text.Length == 0 || PasswordEntry.Text.Length == 0)
        {
            var toast = Toast.Make("Username/password cannot be empty!", ToastDuration.Long);
            await toast.Show();
            LoginBtn.IsEnabled = true;
            return;
        }

        var res = await _account.Login(UsernameEntry.Text, PasswordEntry.Text);
        if (res.HasValue)
        {
            if (res.Value)
            {
                await Shell.Current.GoToAsync("//Home");

                // Clear the inputs after login
                UsernameEntry.Text = "";
                PasswordEntry.Text = "";
            }
            else
            {
                var toast = Toast.Make("Username or password wrong!", ToastDuration.Long);
                await toast.Show();
            }
        }

        LoginBtn.IsEnabled = true;
    }
}