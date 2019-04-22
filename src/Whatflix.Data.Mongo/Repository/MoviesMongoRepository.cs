using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Entities.Movie;
using Whatflix.Data.Abstract.Repository;
using Whatflix.Data.Mongo.Mdo.Movie;
using Whatflix.Infrastructure.ServiceSettings;

namespace Whatflix.Data.Mongo.Repository
{
    public class MoviesMongoRepository : BaseMongoRepository<IMovie, MovieMdo>, IMovieRepository
    {
        public const string COLLECTION_NAME = "movies";
        private IMapper _mapper;

        public MoviesMongoRepository(IOptions<SettingsWrapper> serviceSettings, IMapper mapper) : base(serviceSettings, mapper, COLLECTION_NAME)
        {
            _mapper = mapper;
        }

        public async Task<List<IMovie>> SearchAsync(string[] searchWords)
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

            return _mapper.Map<List<IMovie>>(mongoDataObjects);
        }

        public async Task<List<IMovie>> SearchAsync(string[] searchWords, List<int> movieIds)
        {
            var actorFilter = Builders<MovieMdo>.Filter.AnyIn(f => f.Actors, searchWords);
            var directorFilter = Builders<MovieMdo>.Filter.In(f => f.Director, searchWords);
            var movieFilter = Builders<MovieMdo>.Filter.In(f => f.MovieId, movieIds);

            var filterDefinition = Builders<MovieMdo>.Filter.And(actorFilter, directorFilter, movieFilter);
            var sortDefinition = Builders<MovieMdo>.Sort.Ascending(m => m.Title);
            var findOptions = new FindOptions<MovieMdo, MovieMdo>
            {
                Collation = _caseInsensitiveCollation,
                Sort = sortDefinition,
                Projection = Builders<MovieMdo>.Projection.Include(p => p.Title)
            };

            var movieCusror = await _collection.FindAsync(filterDefinition, findOptions);
            var mongoDataObjects = await movieCusror.ToListAsync();

            return _mapper.Map<List<IMovie>>(mongoDataObjects);
        }

        public async Task UpdatedAppeardInSearchAsync(List<int> movieIds)
        {
            var filterDefinition = Builders<MovieMdo>.Filter.In(f => f.MovieId, movieIds);
            var updateDefinition = Builders<MovieMdo>.Update.Inc(f => f.AppearedInSearches, 1);

            await _collection.UpdateManyAsync(filterDefinition, updateDefinition);
        }

        public async Task<IEnumerable<string>> GetRecommendationByMovieIdsAsync(List<int> movieIds)
        {
            var filterDefinition = Builders<MovieMdo>.Filter.In(f => f.MovieId, movieIds);
            var sortDefinition = Builders<MovieMdo>.Sort.Ascending(m => m.AppearedInSearches);
            var findOptions = new FindOptions<MovieMdo, MovieMdo>
            {
                Sort = sortDefinition,
                Projection = Builders<MovieMdo>.Projection.Include(p => p.Title),
                Limit = 3
            };

            var movieCusror = await _collection.FindAsync(filterDefinition, findOptions);
            var mongoDataObjects = await movieCusror?.ToListAsync();

            return mongoDataObjects.Select(m => m.Title);
        }
    }
}
