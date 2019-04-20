using CsvHelper.Configuration.Attributes;

namespace Whatflix.Api._04_Infrastructure.Helpers
{
    public class MovieMapper
    {
        [Name("id")]
        public int Id { get; set; }

        [Name("original_language")]
        public string OriginalLanguage { get; set; }
    }
}
