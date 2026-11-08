using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
   
    public class CriticReview
    {
        public int Id { get; set; }

        // foreign key
        [Display(Name = "Movie")]
        public int MovieId { get; set; }

        //// navigation goes in both movie and review 
        [ValidateNever]
        [ScaffoldColumn(false)]
        public Movie Movie { get; set; } = null!;

        // not foreign keys or nav properties

        
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Description is required, must be 1000 characters or less")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Rating 0-5 stars")]
        [Range(0, 5, ErrorMessage ="Error: Rating cannot be greater than 5")]
        public int StarRating { get; set; }

        [Required]
        [Display(Name = "Published")]
        public bool IsPublished { get; set; } = false;

        [Required]
        public string CreatedBy { get; set; } = string.Empty;

        [Required]
        [ScaffoldColumn(false)]
        public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);


    }
}
