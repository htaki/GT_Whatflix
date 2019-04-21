using System.Collections.Generic;

namespace Whatflix.Data.Abstract.Entities.Movie
{
    public interface IMovie
    {
        int MovieId { get; set; }
        string Title { get; set; }
        string Language { get; set; }
        string Director { get; set; }
        List<string> Actors { get; set; }
    }
}
