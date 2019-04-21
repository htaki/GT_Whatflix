using Nest;
using System.Collections.Generic;

namespace Whatflix.Data.Elasticsearch.Ado.Movie
{
    [ElasticsearchType(Name = "movies", IdProperty = "MovieId")]
    public class MovieAdo
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public string Director { get; set; }
        public List<string> Actors { get; set; }
    }
}
