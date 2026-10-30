using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is Required")]
        public string Title { get; set; } = string.Empty;


        // foreign key
        [DisplayName("Genre")]
        public int GenreId { get; set; }
     
        [ValidateNever]
        //[ScaffoldColumn(false)]
        public Genre Genre { get; set; } = null!;

        // foreign key
        [DisplayName("Audience Rating")]
        public int AudienceRatingId { get; set; }

        [ValidateNever]
        //[ScaffoldColumn(false)]
        public AudienceRating AudienceRating { get; set; } = null!;

        // navigation property
        //[ValidateNever]
        //[ScaffoldCloumn(false)]
        //public List<CriticReview> Reviews { get; set; } = new List<CriticReview>();

        // not foreign keys

        [Required(ErrorMessage = "Release date is required")]
        [DisplayName("Release Date")]
        public DateOnly ReleaseDate { get; set; }


        [Range(1, 900, ErrorMessage = "Invalid runtime range")]
        [DisplayName("Runtime(Minutes)")]
        public int RuntimeMinutes { get; set; }



        [Required(ErrorMessage ="Country of Origin is required")]
        [DisplayName("Country of Origin")]
        public string OriginCountry { get; set; } = string.Empty;

        [Required(ErrorMessage = "Primary Language is required")]
        public string Language { get; set; } = string.Empty;

        [Required(ErrorMessage ="Synopsis is required")]
        public string Synopsis { get; set; } = string.Empty;

        // [Required]
        public string ImageURL { get; set; } = string.Empty;


    }
}
