using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mission__6_Assignment.Models
{
    public class Movie
    {
        public int MovieId { get; set; }

        // Category is a foreign key to Categories table
        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Range(1888, 3000, ErrorMessage = "Year must be 1888 or later.")]
        public int Year { get; set; }

        public string? Director { get; set; }
        public string? Rating { get; set; }

        [Required]
        public bool Edited { get; set; }

        public string? LentTo { get; set; }

        [Required]
        public bool CopiedToPlex { get; set; }

        public string? Notes { get; set; }


        public int GetMovieId()
        {
            return MovieId;
        }

        public void SetMovieId(int movieId)
        {
            MovieId = movieId;
        }

    }
}
