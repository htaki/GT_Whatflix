using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Whatflix.Api._03_Data.Mdo.UserPreference.Internal
{
    public class PreferedMovieMdo
    {
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Language { get; set; }

        [DataMember]
        public string Director { get; set; }

        [DataMember]
        public List<string> Actors { get; set; }
    }
}
