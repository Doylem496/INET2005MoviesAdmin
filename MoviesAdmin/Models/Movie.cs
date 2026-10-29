using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is Required")]
        public string Title { get; set; } = string.Empty;


        // foreign key
        // [Display(name = "Genre")]
        public int GenreId { get; set; }
     
        [ValidateNever]
        public Genre Genre { get; set; } = null!;

        // foreign key
        // [Display(name = "Audience Rating")]
        public int AudienceRatingId { get; set; }

        [ValidateNever]
        public AudienceRating AudienceRating { get; set; } = null!;

        // not foreign keys

        [Required(ErrorMessage = "Release date is required")]
        public DateOnly ReleaseDate { get; set; }

        [Range(1, 900, ErrorMessage = "Invalid runtime range")]
        public int RuntimeMinutes { get; set; }

        [Required(ErrorMessage ="Country of Origin is required")]
        public string OriginCountry { get; set; } = string.Empty;

        [Required(ErrorMessage = "Primary Language is required")]
        public string Language { get; set; } = string.Empty;

        [Required(ErrorMessage ="Synopsis is required")]
        public string Synopsis { get; set; } = string.Empty;

        // [Required]
        public string ImageURL { get; set; } = string.Empty;


    }
}
