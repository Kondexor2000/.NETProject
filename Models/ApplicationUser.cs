using Microsoft.AspNetCore.Identity;
using MyApp.Models;

public class ApplicationUser : IdentityUser
{
    public ICollection<Posts> Posts { get; set; }
    public ICollection<Comments> Comments { get; set; }
}