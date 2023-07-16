using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using PostlyApp.Enums;
using PostlyApp.Models.DTOs;
using PostlyApp.Services;
using PostlyApp.ViewModels;

namespace PostlyApp.Views;

public partial class PostView : ContentView
{

    bool voteChangeLoading = false;
    private readonly IContentService _content;

    public PostView(PostDTO post)
    {
        InitializeComponent();
        BindingContext = new PostViewViewModel(post);
        _content = DependencyService.Resolve<IContentService>();
    }

    /// <summary>
    /// Called when the upvote btn is clicked, tries to submit the upvote.
    /// </summary>
    private async void UpvoteTapped(object sender, TappedEventArgs e)
    {
        if (voteChangeLoading || BindingContext is not PostViewViewModel viewModel)
        {
            return;
        }
        voteChangeLoading = true;

        if (viewModel.Post.Vote == VoteType.Upvote)
        {
            var update = await _content.RemoveVote(viewModel.Post.Id);
            await UpdateVotes(update, viewModel);
        }
        else
        {
            var update = await _content.SetVote(viewModel.Post.Id, VoteType.Upvote);
            await UpdateVotes(update, viewModel);
        }

        voteChangeLoading = false;
    }

    /// <summary>
    /// Called when the downvote btn is clicked, tries to submit the downvote.
    /// </summary>
    private async void DownvoteTapped(object sender, TappedEventArgs e)
    {
        if (voteChangeLoading || BindingContext is not PostViewViewModel viewModel)
        {
            return;
        }
        voteChangeLoading = true;

        if (viewModel.Post.Vote == VoteType.Downvote)
        {
            var update = await _content.RemoveVote(viewModel.Post.Id);
            await UpdateVotes(update, viewModel);
        }
        else
        {
            var update = await _content.SetVote(viewModel.Post.Id, VoteType.Downvote);
            await UpdateVotes(update, viewModel);
        }

        voteChangeLoading = false;
    }

    /// <summary>
    /// Helper method to update the view after an upvote/downvote occured.
    /// </summary>
    /// <param name="update">The update to process.</param>
    /// <param name="model">The model to apply the update to.</param>
    /// <returns></returns>

    private async Task UpdateVotes(VoteUpdateViewModel? update, PostViewViewModel model)
    {
        if (update != null)
        {
            model.Post.UpvoteCount = update.UpvoteCount;
            model.Post.DownvoteCount = update.DownvoteCount;
            model.Post.Vote = update.VoteType;
        }
        else
        {
            var toast = Toast.Make("Error submitting your vote!");
            await toast.Show();
        }
    }

    /// <summary>
    /// Called when the comment btn is clicked, opens up the popup with the postdetails and comments.
    /// </summary>
    private void CommentsBtnTapped(object sender, TappedEventArgs e)
    {
        if (BindingContext is PostViewViewModel viewModel)
        {
            Shell.Current.ShowPopup(new PostdetailView(viewModel.Post));
        }
    }
}