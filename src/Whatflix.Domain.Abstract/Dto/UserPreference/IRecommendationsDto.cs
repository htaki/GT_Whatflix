using System.Collections.Generic;

namespace Whatflix.Domain.Abstract.Dto.UserPreference
{
    public interface IRecommendationsDto
    {
        int User { get; set; }
        IEnumerable<string> Movies { get; set; }
    }
}
