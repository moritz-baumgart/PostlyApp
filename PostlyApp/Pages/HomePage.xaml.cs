using PostlyApp.Services;

namespace PostlyApp.Pages;

public partial class HomePage : ContentPage
{
    private readonly IContentService _content;

    public HomePage()
    {
        InitializeComponent();
        _content = DependencyService.Resolve<IContentService>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        publicFeed.Posts = await _content.GetPublicFeed(null);
    }
}