using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Entities.Movie;
using Whatflix.Data.Abstract.Repository;
using Whatflix.Data.Mongo.Mdo.Movie;
using Whatflix.Infrastructure.ServiceSettings;

namespace Whatflix.Data.Mongo.Repository
{
    public class MoviesMongoRepository : BaseMongoRepository<IMovieEntity, MovieMdo>, IMovieRepository
    {
        public const string COLLECTION_NAME = "movies";

        public MoviesMongoRepository(IOptions<SettingsWrapper> serviceSettings, 
            IMapper mapper) : base(serviceSettings, mapper, COLLECTION_NAME)
        {
        }

        public async Task<List<IMovieEntity>> SearchAsync(string[] searchWords)
        {
            var filterDefinition = GetSearchWordsFilter(searchWords);
            var findOptions = new FindOptions<MovieMdo, MovieMdo>
            {
                Collation = _caseInsensitiveCollation,
                Sort = Builders<MovieMdo>.Sort.Ascending(m => m.Title),
                Projection = Builders<MovieMdo>.Projection.Include(p => p.Title)
            };

            return await FindAsync(filterDefinition, findOptions);
        }

        public async Task<List<IMovieEntity>> SearchAsync(string[] searchWords,
            List<string> favoriteActors,
            List<string> favoriteDirectors,
            List<string> preferredLanguages)
        {
            var searchFilter = GetSearchWordsFilter(searchWords);
            var userPreferenceFilter = GetUserPreferenceFilter(favoriteActors, favoriteDirectors, preferredLanguages);
            var filterDefinition = Builders<MovieMdo>.Filter.And(searchFilter, userPreferenceFilter);

            var findOptions = new FindOptions<MovieMdo, MovieMdo>
            {
                Collation = _caseInsensitiveCollation,
                Sort = Builders<MovieMdo>.Sort.Ascending(m => m.Title),
                Projection = Builders<MovieMdo>.Projection.Include(p => p.Title)
            };

            return await FindAsync(filterDefinition, findOptions);
        }

        public async Task UpdateAppeardInSearchAsync(List<int> movieIds)
        {
            var filterDefinition = Builders<MovieMdo>.Filter.In(f => f.MovieId, movieIds);
            var updateDefinition = Builders<MovieMdo>.Update.Inc(f => f.AppearedInSearches, 1);

            await _collection.UpdateManyAsync(filterDefinition, updateDefinition);
        }

        public async Task<List<IMovieEntity>> GetRecommendationsAsync(List<string> favoriteActors,
            List<string> favoriteDirectors,
            List<string> preferredLanguages)
        {
            var filterDefinition = GetUserPreferenceFilter(favoriteActors, favoriteDirectors, preferredLanguages);
            var findOptions = new FindOptions<MovieMdo, MovieMdo>
            {
                Collation = _caseInsensitiveCollation,
                Sort = Builders<MovieMdo>.Sort.Descending(m => m.AppearedInSearches),
                Projection = Builders<MovieMdo>.Projection.Include(p => p.Title),
                Limit = 3
            };

            return await FindAsync(filterDefinition, findOptions);
        }

        #region Private Methods

        private FilterDefinition<MovieMdo> GetSearchWordsFilter(string[] searchWords)
        {
            var actorFilter = Builders<MovieMdo>.Filter.AnyIn(f => f.Actors, searchWords);
            var directorFilter = Builders<MovieMdo>.Filter.In(f => f.Director, searchWords);
            var titleFilter = Builders<MovieMdo>.Filter.In(f => f.Title, searchWords);

            return Builders<MovieMdo>.Filter.Or(actorFilter, directorFilter, titleFilter);
        }

        private FilterDefinition<MovieMdo> GetUserPreferenceFilter(List<string> favoriteActors, List<string> favoriteDirectors, List<string> preferredLanguages)
        {
            var favoriteActorFilter = Builders<MovieMdo>.Filter.AnyIn(f => f.Actors, favoriteActors);
            var favoriteDirectorFilter = Builders<MovieMdo>.Filter.In(f => f.Director, favoriteDirectors);
            var favoriteLanguageFilter = Builders<MovieMdo>.Filter.In(f => f.Language, preferredLanguages);

            return Builders<MovieMdo>.Filter.And(Builders<MovieMdo>.Filter.Or(favoriteActorFilter, favoriteDirectorFilter), favoriteLanguageFilter);
        }

        #endregion
    }
}
