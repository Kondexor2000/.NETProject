using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyApp.ViewModels
{
    public class PostUpdateView
    {   
        [Required]
        public int SelectedPostId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int SelectedCategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        [Required] 
        [StringLength(100, ErrorMessage = "The name cannot be longer than 100 characters.")]
        public string Title { get; set; }

        [Required] 
        [StringLength(500, ErrorMessage = "The description cannot be longer than 500 characters.")]
        public string Description { get; set; }
    }
}