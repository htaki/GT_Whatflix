using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whatflix.Api._03_Data.Mdo.Movie;
using Whatflix.Api._03_Data.Mdo.UserPreference;

namespace Whatflix.Api._03_Data.Repository
{
    public class MoviesRepository : BaseRepository<MovieMdo>
    {
        public const string COLLECTION_NAME = "movies";

        public MoviesRepository(IOptions<ServiceSettings> serviceSettings)
            : base(serviceSettings.Value.Whatfix_Db.ConnectionString, COLLECTION_NAME)
        {
        }

        public async Task<List<MovieMdo>> Search(string[] searchWords)
        {
            var actorFilter = Builders<MovieMdo>.Filter.AnyIn(f => f.Actors, searchWords);
            var titleFilter = Builders<MovieMdo>.Filter.In(f => f.Title, searchWords);
            var directorFilter = Builders<MovieMdo>.Filter.In(f => f.Director, searchWords);

            var filterDefinition = Builders<MovieMdo>.Filter.Or(actorFilter, titleFilter, directorFilter);
            var sortDefinition = Builders<MovieMdo>.Sort.Ascending(m => m.Title);
            var findOptions = new FindOptions<MovieMdo, MovieMdo>
            {
                Collation = _caseInsensitiveCollation,
                Sort = sortDefinition,
                Projection = Builders<MovieMdo>.Projection.Include(p => p.Title)
            };

            var movieCusror = await _collection.FindAsync(filterDefinition, findOptions);
            return await movieCusror.ToListAsync();
        }

        public async Task<List<MovieMdo>> Search(string[] searchWords, UserPreferenceMdo userPreference)
        {
            var actors = searchWords.Where(sw => userPreference.FavoriteActors.Any(p => string.Equals(p, sw, StringComparison.OrdinalIgnoreCase)));
            var directors = searchWords.Where(sw => userPreference.FavoriteDirectors.Any(p => string.Equals(p, sw, StringComparison.OrdinalIgnoreCase)));

            var actorFilter = Builders<MovieMdo>.Filter.AnyIn(f => f.Actors, actors);
            var directorFilter = Builders<MovieMdo>.Filter.In(f => f.Director, directors);
            var languageFilter = Builders<MovieMdo>.Filter.In(f => f.Language, userPreference.PreferedLanguages);

            var filterDefinition = Builders<MovieMdo>.Filter.And(actorFilter, directorFilter, languageFilter);
            var sortDefinition = Builders<MovieMdo>.Sort.Ascending(m => m.Title);
            var findOptions = new FindOptions<MovieMdo, MovieMdo>
            {
                Collation = _caseInsensitiveCollation,
                Sort = sortDefinition,
                Projection = Builders<MovieMdo>.Projection.Include(p => p.Title)
            };

            var movieCusror = await _collection.FindAsync(filterDefinition, findOptions);
            return await movieCusror.ToListAsync();
        }
    }
}
