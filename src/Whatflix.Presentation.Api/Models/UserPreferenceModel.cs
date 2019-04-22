using System.Collections.Generic;

namespace Whatflix.Presentation.Api.Models
{
    public class UserPreferenceModel
    {
        public int UserId { get; set; }
        public List<int> MovieIds { get; set; }
    }
}
