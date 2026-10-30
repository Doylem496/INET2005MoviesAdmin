using Microsoft.EntityFrameworkCore;

public class MoviesAdminContext(DbContextOptions<MoviesAdminContext> options) : DbContext(options)
{
    public DbSet<MoviesAdmin.Models.Movie> Movie { get; set; } = default!;
    public DbSet<MoviesAdmin.Models.Genre> Genres { get; set; } = default!;

    public DbSet<MoviesAdmin.Models.AudienceRating> AudienceRatings { get; set; } = default!;

    // uncomment once scaffolded
    //public DbSet<MoviesAdmin.Models.CriticReview> CriticReviews { get; set; } = default!;
}
