namespace PostlyApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell(); ;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);
        window.Width = 720;
        window.Height = 1280 - 20;
        return window;
    }
}
