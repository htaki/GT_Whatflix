using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Whatflix.Api._03_Data.Mdo.UserPreference
{
    public class UserPreferenceMdo : BaseMdo
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public List<string> PreferedLanguages { get; set; }

        [DataMember]
        public List<string> FavoriteActors { get; set; }

        [DataMember]
        public List<string> FavoriteDirectors { get; set; }
    }
}
