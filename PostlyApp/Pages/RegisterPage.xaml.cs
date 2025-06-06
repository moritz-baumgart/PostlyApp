using CommunityToolkit.Maui.Alerts;
using PostlyApp.Services;

namespace PostlyApp.Pages;

public partial class RegisterPage : ContentPage
{
    private readonly IAccountService _account;

    public RegisterPage()
    {
        InitializeComponent();
        _account = DependencyService.Resolve<IAccountService>();
    }

    /// <summary>
    /// Is called when the login btn is clicked, navigates the user to the login page.
    /// </summary>
    private async void OnSigninClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Login");
    }

    /// <summary>
    /// Is called when the create account btn is clicked. Tries to register the user, displays warnings if username/pw is empty, if passwords do not match and if the username is already taken.
    /// </summary>
    private async void Create_Clicked(object sender, EventArgs e)
    {

        if (Username.Text == null || Username.Text.Length == 0)
        {
            var toast = Toast.Make("Username cannot be empty!");
            await toast.Show();
            return;
        }

        if (PW1.Text == null || PW2.Text == null || PW1.Text.Length == 0)
        {
            var toast = Toast.Make("Password cannot be empty!");
            await toast.Show();
            return;
        }

        if (!PW1.Text.Equals(PW2.Text))
        {
            var toast = Toast.Make("Password do not match!");
            await toast.Show();
            return;
        }

        

        var res = await _account.Register(Username.Text, PW1.Text);

        if (res != null)
        {
            if ((bool)res)
            {
                var toast = Toast.Make("Account created! You can now login.");
                await toast.Show();
                await Shell.Current.GoToAsync("//Login");
            }
            else
            {
                var toast = Toast.Make("Username already in use!");
                await toast.Show();
            }
        }
        else
        {
            var toast = Toast.Make("An error occured!");
            await toast.Show();
        }

    }
}