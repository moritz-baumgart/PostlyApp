using PostlyApi.Enums;
using PostlyApi.Models.DTOs;

namespace PostlyApp.Pages;

public partial class HomePage : ContentPage
{
    List<PostDTO> postList = new();

    public HomePage()
    {
        InitializeComponent();

        // Set the posts, for now lets just generate some mockup data
        var someAuthor = new UserDTO
        {
            Id = 1,
            Username = "TestUser",
            DisplayName = "ThatsMyName"
        };

        for (int i = 0; i < 10; i++)
        {
            postList.Add(new PostDTO
            {
                Author = someAuthor,
                Content = "This is a test" + i,
                CommentCount = 5,
                UpvoteCount = 1,
                DownvoteCount = 3,
                CreatedAt = DateTime.Now,
                Vote = VoteType.Upvote,
                HasCommented = true
            });
        };
        // end of mockup data

        publicFeed.Posts = postList;
    }
}