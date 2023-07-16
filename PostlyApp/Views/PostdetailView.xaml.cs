using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using PostlyApp.Models.DTOs;
using PostlyApp.Services;
using PostlyApp.ViewModels;

namespace PostlyApp.Views;

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

    /// <summary>
    /// Lifecylce method called when the popup is opened. Fetches the comments of the post.
    /// </summary>
    private async void PopupOpened(object? sender, PopupOpenedEventArgs e)
    {
        await LoadComments();
    }

    /// <summary>
    /// Helper method to fetch the comments.
    /// </summary>
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

    /// <summary>
    /// Called when the discard button is clicked. Closes the popup.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Discard(object sender, EventArgs e)
    {
        Close();
    }

    /// <summary>
    /// Called when the comment btn is clicked. Tries to submit the comment.
    /// </summary>
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