namespace MovieCatalogue.ViewModels
{
    public class MovieDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Director { get; set; }

        public string Genre { get; set; }

        public int ReleaseYear { get; set; }

        public int Duration { get; set; }

        public double Rating { get; set; }

        public string Language { get; set; }

        public string Description { get; set; }
    }
}