using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using PostlyApp.Services;
using PostlyApp.ViewModels;

namespace Postly.Views;

public partial class PostdetailView : Popup
{
    private readonly object _content;

    public PostdetailView(long postId)
    {
        InitializeComponent();
        _content = DependencyService.Resolve<IContentService>();
        BindingContext = new PostdetailViewModel();
        postDetailPopup.Opened += PopupOpened;
    }

    private async void PopupOpened(object? sender, PopupOpenedEventArgs e)
    {

    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}