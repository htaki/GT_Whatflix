using System.Collections.Generic;

namespace Whatflix.Data.Abstract.Entities.UserPreference
{
    public interface IUserPreference
    {
        int UserId { get; set; }
        List<int> MovieIds { get; set; }
    }
}
