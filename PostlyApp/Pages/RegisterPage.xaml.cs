using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using PostlyApp.Services;

namespace PostlyApp.Pages;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
	{
        InitializeComponent();
    }
    
    private async void OnSigninClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Login");
    }
}