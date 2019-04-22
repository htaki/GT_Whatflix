using CsvHelper.Configuration.Attributes;

namespace Whatflix.Infrastructure.Helpers.Mappers
{
    public class MovieMapper
    {
        [Name("id")]
        public int Id { get; set; }

        [Name("original_language")]
        public string OriginalLanguage { get; set; }
    }
}
