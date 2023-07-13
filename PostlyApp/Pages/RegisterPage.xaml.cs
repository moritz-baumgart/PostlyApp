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

    private void Create_Clicked(object sender, EventArgs e)
    {

    }
}