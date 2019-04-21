using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Whatflix.Data.Abstract.Entities.Movie;

namespace Whatflix.Data.Mongo.Mdo.Movie
{
    public class MovieMdo : IMovie
    {
        [BsonId]
        public int MovieId { get; set; }

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
