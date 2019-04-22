using System.Collections.Generic;

namespace Whatflix.Data.Abstract.Entities.UserPreference
{
    public interface IUserPreferenceEntity
    {
        int UserId { get; set; }
        List<int> MovieIds { get; set; }
    }
}
