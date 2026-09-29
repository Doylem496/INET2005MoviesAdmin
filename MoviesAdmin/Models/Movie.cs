namespace MoviesAdmin.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Genre { get; set; } = string.Empty;

        public DateOnly ReleaseDate { get; set; }

        public int RuntimeMinutes { get; set; }

        public double CriticScore { get; set; }

        public double UserScore { get; set; }

        public string OriginCountry { get; set; } = string.Empty;

        public string Language { get; set; } = string.Empty;

        public string Synopsis { get; set; } = string.Empty;

        public string MPAARating { get; set; } = string.Empty;


    }
}
