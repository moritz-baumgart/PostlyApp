using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using Postly.Views;
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

    private void CommentsBtnTapped(object sender, TappedEventArgs e)
    {
        if (BindingContext is PostViewViewModel viewModel)
        {
            Shell.Current.ShowPopup(new PostdetailView(viewModel.Post));
        }
    }
}