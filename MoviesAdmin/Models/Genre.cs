using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Genre
    {
        
        public int Id { get; set; }


        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [ValidateNever]
        public List<Movie> Movies { get; set; } = new List<Movie>();


    }
}
