using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Models
{
    public class Posts
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Categories { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public ICollection<Comments> Comments { get; set; }

    }
}