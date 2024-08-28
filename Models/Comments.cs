using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Models
{
    public class Comments
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public int PostId { get; set; }

        [ForeignKey("PostId")]
        public Posts Post { get; set; }
    
        public string Title { get; set; }

    }
}