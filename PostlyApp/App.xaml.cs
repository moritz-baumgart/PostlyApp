namespace PostlyApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        // Initialize the app shell.
        MainPage = new AppShell(); ;
    }

    /// <summary>
    /// For our demonstration we used the windows machine, this override scales it's window to a "smartphone like" format.
    /// </summary>
    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);
        window.Width = 720;
        window.Height = 1280 - 20; // - 20 because of the title bar.
        return window;
    }
}
