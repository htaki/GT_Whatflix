using System.Collections.Generic;

namespace Whatflix.Domain.Dto.Movie
{
    public class MovieDto
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public string Director { get; set; }
        public List<string> Actors { get; set; }
    }
}
