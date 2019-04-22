using Nest;
using System.Collections.Generic;
using Whatflix.Data.Abstract.Entities.Movie;

namespace Whatflix.Data.Elasticsearch.Ado.Movie
{
    [ElasticsearchType(Name = "movies", IdProperty = "MovieId")]
    public class MovieAdo : IMovie
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public string Director { get; set; }
        public List<string> Actors { get; set; }
        public int AppearedInSearches { get; set; }
    }
}
