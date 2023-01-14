using BugTrack.Models;

namespace BugTrack.ViewModels.VMComments
{
    public class CommentViewModel
    {
        public string? OwnerName { get; set; }
        public int CommentId { get; set; }
        public string BugUserId { get; set; }
        public string Content { get; set; }
        public DateTime TimePosted { get; set; }

        public CommentViewModel() { }
        public static CommentViewModel CommentToVM(Comment comment)
        {
            var commentViewModel = new CommentViewModel();
            commentViewModel.TimePosted = comment.TimePosted;
            commentViewModel.BugUserId = comment.BugUserId;
            commentViewModel.Content = comment.Content;
            commentViewModel.CommentId = comment.Id;
            commentViewModel.OwnerName = comment.OwnerName;
            return commentViewModel;
        }
    }
}
