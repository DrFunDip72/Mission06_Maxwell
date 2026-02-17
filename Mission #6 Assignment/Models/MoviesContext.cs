using Microsoft.EntityFrameworkCore;

namespace Mission__6_Assignment.Models
{
    public class MoviesContext : DbContext
    {
        public MoviesContext (DbContextOptions<MoviesContext> options) : base(options)
        {
        }   

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().ToTable("Movies");
            modelBuilder.Entity<Category>().ToTable("Categories");
        }

    }
}
