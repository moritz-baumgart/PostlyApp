using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using PostlyApp.Services;

namespace PostlyApp.Views;

public partial class NewPostPopup : Popup
{
    private readonly IContentService _content;

    public NewPostPopup()
    {
        InitializeComponent();
        _content = DependencyService.Resolve<IContentService>();
    }

    private async void PostButtonClicked(object sender, EventArgs e)
    {
        var text = newPostText.Text;

        if (text == null)
        {
            return;
        }

        if (text.Length == 0)
        {
            return;
        }

        var postId = await _content.AddPost(text);

        if (postId != null)
        {
            Close();
            newPostText.Text = "";
        }
        else
        {
            var toast = Toast.Make("Error creating post!");
            await toast.Show();
        }
    }
}