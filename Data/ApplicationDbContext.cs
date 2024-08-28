using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyApp.Models;

namespace NetflixSharp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Posts> Posts { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Posts>()
                   .HasOne(p => p.User)
                   .WithMany(u => u.Posts)
                   .HasForeignKey(p => p.UserId);

            builder.Entity<Posts>()
                   .HasOne(p => p.Categories)
                   .WithMany(c => c.Posts)
                   .HasForeignKey(p => p.CategoryId);

            builder.Entity<Comments>()
                   .HasOne(c => c.User)
                   .WithMany(u => u.Comments)
                   .HasForeignKey(c => c.UserId);

            builder.Entity<Comments>()
                   .HasOne(c => c.Post)
                   .WithMany(p => p.Comments)
                   .HasForeignKey(c => c.PostId);

        }

        internal async Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}