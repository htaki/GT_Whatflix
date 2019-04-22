using CsvHelper.Configuration.Attributes;

namespace Whatflix.Infrastructure.Helpers.Mappers
{
    public class CreditsMapper
    {
        [Name("movie_id")]
        public int MovieId { get; set; }

        [Name("title")]
        public string Title { get; set; }

        [Name("cast")]
        public string Cast { get; set; }

        [Name("crew")]
        public string Crew { get; set; }
    }
}
