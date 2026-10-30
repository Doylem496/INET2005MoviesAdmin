using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class CriticReview
    {
        public int Id;

        // foreign key
        //public int MovieId { get; set; }

        //// navigation goes in both movie and review 
        //[ValidateNever]
        //[ScaffoldColumn(false)]
        //public Movie Movie { get; set; } = null!;

        // not foreign keys or nav properties

        [Required(ErrorMessage ="Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1, 5, ErrorMessage ="Error: Rating cannot be greater than 5")]
        public int StarRating { get; set; }

        [Required]
        public bool IsPublished { get; set; }

        [Required]
        public string CreatedBy { get; set; } = string.Empty;

        [Required]
        // [ScaffoldColumn(false)]
        public DateOnly CreatedDate { get; set; }


    }
}
