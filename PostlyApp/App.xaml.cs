using PostlyApp.Services;
using PostlyApp.Services.Impl;

namespace PostlyApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell(); ;
    }
}
