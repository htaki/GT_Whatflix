using System.Collections.Generic;

namespace Whatflix.Domain.Dto.UserPreference
{
    public class UserPreferenceDto
    {
        public int UserId { get; set; }
        public List<int> MovieIds { get; set; }
    }
}
