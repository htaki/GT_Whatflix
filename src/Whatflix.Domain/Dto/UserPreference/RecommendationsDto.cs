using System.Collections.Generic;
using Whatflix.Domain.Abstract.Dto.UserPreference;

namespace Whatflix.Domain.Dto.UserPreference
{
    public class RecommendationsDto : IRecommendationsDto
    {
        public int User { get; set; }
        public IEnumerable<string> Movies { get; set; }
    }
}
