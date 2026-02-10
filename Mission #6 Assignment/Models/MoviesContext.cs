using Microsoft.EntityFrameworkCore;

namespace Mission__6_Assignment.Models
{
    public class MoviesContext : DbContext
    {
        public MoviesContext (DbContextOptions<MoviesContext> options) : base(options)
        {
        }   

        public DbSet<Movie> Movies { get; set; }

    }
}
