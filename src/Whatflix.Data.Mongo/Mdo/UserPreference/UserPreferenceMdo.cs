using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using Whatflix.Data.Abstract.Entities.UserPreference;

namespace Whatflix.Data.Mongo.Mdo.UserPreference
{
    public class UserPreferenceMdo : IUserPreferenceEntity
    {
        [BsonId]
        public int UserId { get; set; }
        public List<int> MovieIds { get; set; }
    }
}
