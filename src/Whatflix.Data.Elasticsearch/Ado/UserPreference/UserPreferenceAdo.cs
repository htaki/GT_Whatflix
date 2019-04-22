using Nest;
using System.Collections.Generic;
using Whatflix.Data.Abstract.Entities.UserPreference;

namespace Whatflix.Data.Mongo.Ado.UserPreference
{
    [ElasticsearchType(Name = "user-preference", IdProperty = "UserId")]
    public class UserPreferenceAdo : IUserPreference
    {
        public int UserId { get; set; }
        public List<int> MovieIds { get; set; }
    }
}
