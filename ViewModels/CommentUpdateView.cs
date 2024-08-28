using System.ComponentModel.DataAnnotations;

namespace MyApp.ViewModels
{
    public class CommentUpdateView
    {
        [Required]
        public int RequestUser { get; set; }

        [Required]
        public int SelectedPostId { get; set; }
        public string Title { get; set; }
    }
}