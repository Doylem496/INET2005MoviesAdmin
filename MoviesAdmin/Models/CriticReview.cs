using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class CriticReview
    {
        public int Id;

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
        public DateOnly CreatedDate { get; set; }


    }
}
