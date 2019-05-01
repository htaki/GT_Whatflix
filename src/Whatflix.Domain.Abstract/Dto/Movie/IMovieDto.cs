using System.Collections.Generic;

namespace Whatflix.Domain.Abstract.Dto.Movie
{
    public interface IMovieDto
    {
        int MovieId { get; set; }
        string Title { get; set; }
        string Language { get; set; }
        string Director { get; set; }
        List<string> Actors { get; set; }
    }
}
