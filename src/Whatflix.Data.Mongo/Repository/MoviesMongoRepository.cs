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
        private IMapper _mapper;

        public MoviesMongoRepository(IOptions<SettingsWrapper> serviceSettings, IMapper mapper) : base(serviceSettings, mapper, COLLECTION_NAME)
        {
            _mapper = mapper;
        }

        public async Task<List<IMovieEntity>> SearchAsync(string[] searchWords)
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
            var mongoDataObjects = await movieCusror.ToListAsync();

            return _mapper.Map<List<IMovieEntity>>(mongoDataObjects);
        }

        public async Task<List<IMovieEntity>> SearchAsync(string[] searchWords,
            List<string> favoriteActors,
            List<string> favoriteDirectors,
            List<string> favoriteLanguages)
        {
            var actorFilter = Builders<MovieMdo>.Filter.AnyIn(f => f.Actors, searchWords);
            var directorFilter = Builders<MovieMdo>.Filter.In(f => f.Director, searchWords);
            var titleFilter = Builders<MovieMdo>.Filter.In(f => f.Title, searchWords);
            var searchFilter = Builders<MovieMdo>.Filter.Or(actorFilter, directorFilter, titleFilter);

            var favoriteActorFilter = Builders<MovieMdo>.Filter.AnyIn(f => f.Actors, favoriteActors);
            var favoriteDirectorFilter = Builders<MovieMdo>.Filter.In(f => f.Director, favoriteDirectors);
            var favoriteLanguageFilter = Builders<MovieMdo>.Filter.In(f => f.Language, favoriteLanguages);
            var userPreferenceFilter = Builders<MovieMdo>.Filter.And(Builders<MovieMdo>.Filter.Or(favoriteActorFilter, favoriteDirectorFilter), favoriteLanguageFilter);

            var filterDefinition = Builders<MovieMdo>.Filter.And(searchFilter, userPreferenceFilter);

            var sortDefinition = Builders<MovieMdo>.Sort.Ascending(m => m.Title);
            var findOptions = new FindOptions<MovieMdo, MovieMdo>
            {
                Collation = _caseInsensitiveCollation,
                Sort = sortDefinition,
                Projection = Builders<MovieMdo>.Projection.Include(p => p.Title)
            };

            var movieCusror = await _collection.FindAsync(filterDefinition, findOptions);
            var mongoDataObjects = await movieCusror.ToListAsync();

            return _mapper.Map<List<IMovieEntity>>(mongoDataObjects);
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
            var favoriteActorFilter = Builders<MovieMdo>.Filter.AnyIn(f => f.Actors, favoriteActors);
            var favoriteDirectorFilter = Builders<MovieMdo>.Filter.In(f => f.Director, favoriteDirectors);
            var favoriteLanguageFilter = Builders<MovieMdo>.Filter.In(f => f.Language, preferredLanguages);
            var filterDefinition = Builders<MovieMdo>.Filter.And(Builders<MovieMdo>.Filter.Or(favoriteActorFilter, favoriteDirectorFilter), favoriteLanguageFilter);

            var sortDefinition = Builders<MovieMdo>.Sort.Descending(m => m.AppearedInSearches);
            var findOptions = new FindOptions<MovieMdo, MovieMdo>
            {
                Collation = _caseInsensitiveCollation,
                Sort = sortDefinition,
                Projection = Builders<MovieMdo>.Projection.Include(p => p.Title),
                Limit = 3
            };

            var movieCusror = await _collection.FindAsync(filterDefinition, findOptions);
            var mongoDataObjects = await movieCusror.ToListAsync();

            return _mapper.Map<List<IMovieEntity>>(mongoDataObjects);
        }
    }
}
