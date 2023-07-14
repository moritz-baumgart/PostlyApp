using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using PostlyApp.Models.DTOs;
using PostlyApp.Services;
using PostlyApp.ViewModels;

namespace Postly.Views;

public partial class PostdetailView : Popup
{
    private readonly IContentService _content;

    public PostdetailView(PostDTO post)
    {
        InitializeComponent();
        _content = DependencyService.Resolve<IContentService>();
        BindingContext = new PostdetailViewModel(post);
        postDetailPopup.Opened += PopupOpened;
    }

    private async void PopupOpened(object? sender, PopupOpenedEventArgs e)
    {
        await LoadComments();
    }

    private async Task LoadComments()
    {
        if (BindingContext is PostdetailViewModel viewModel)
        {
            var res = await _content.GetComments(viewModel.Post.Id);
            if (res != null)
            {
                viewModel.Comments = res;
            }
            else
            {
                var toast = Toast.Make("Error loading comments!");
                await toast.Show(); 
            }
        }
    }

    private void Discard(object sender, EventArgs e)
    {
        Close();
    }

    private async void CreateComment(object sender, EventArgs e)
    {
        newCommentBtn.IsEnabled = false;

        if (BindingContext is PostdetailViewModel viewModel)
        {
            var res = await _content.AddComment(viewModel.Post.Id, commentEditor.Text);
            if (res != null)
            {
                commentEditor.Text = "";
                await LoadComments();
            }
            else
            {
                var toast = Toast.Make("Error creating comment!");
                await toast.Show();
            }
        }

        newCommentBtn.IsEnabled = true;
    }
}