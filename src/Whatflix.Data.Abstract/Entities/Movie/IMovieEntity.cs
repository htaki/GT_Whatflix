using System.Collections.Generic;

namespace Whatflix.Data.Abstract.Entities.Movie
{
    public interface IMovieEntity
    {
        int MovieId { get; set; }
        string Title { get; set; }
        string Language { get; set; }
        string Director { get; set; }
        List<string> Actors { get; set; }
        int AppearedInSearches { get; set; }
    }
}
