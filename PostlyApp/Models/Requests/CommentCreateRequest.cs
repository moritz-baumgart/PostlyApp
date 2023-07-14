namespace PostlyApp.Models.Requests
{
    public class CommentCreateRequest
    {
        public int PostId { get; set; }
        public string CommentContent { get; set; }
    }
}
