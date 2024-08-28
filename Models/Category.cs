using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        // Relacje
        public ICollection<Posts> Posts { get; set; }
    }
}