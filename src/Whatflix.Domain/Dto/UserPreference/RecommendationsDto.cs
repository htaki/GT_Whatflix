using System.Collections.Generic;

namespace Whatflix.Domain.Dto.UserPreference
{
    public class RecommendationsDto
    {
        public int User { get; set; }
        public IEnumerable<string> Movies { get; set; }
    }
}
