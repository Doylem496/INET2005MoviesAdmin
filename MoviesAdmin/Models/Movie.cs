using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is Required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Genre is Required")]
        public string Genre { get; set; } = string.Empty;

        public DateOnly ReleaseDate { get; set; }

        [Range(1, 900, ErrorMessage = "Invalid runtime range")]
        public int RuntimeMinutes { get; set; }

        [Range(0,100, ErrorMessage = "Critic score must be between 0 and 100")]
        public double CriticScore { get; set; }

        [Range(0,100, ErrorMessage = "User score must be between 0 and 100")]
        public double UserScore { get; set; }

        [Required(ErrorMessage ="Country of Origin is required")]
        public string OriginCountry { get; set; } = string.Empty;

        [Required(ErrorMessage = "Primary Language is required")]
        public string Language { get; set; } = string.Empty;

        [Required(ErrorMessage ="Synopsis is required")]
        public string Synopsis { get; set; } = string.Empty;

        [Required(ErrorMessage ="Enter NR if the movie was not rated by the MPAA")]
        public string MPAARating { get; set; } = string.Empty;

        // [Required]
        public string ImageURL { get; set; } = string.Empty;


    }
}
