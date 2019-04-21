using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
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

        public async Task<List<IMovie>> Search(string[] searchWords)
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

        public async Task<List<IMovie>> Search(string[] searchWords, string[] favoriteActors, string[] favoriteDirectors, string[] favoriteLanguages)
        {
            var actors = searchWords.Where(sw => favoriteActors.Any(p => string.Equals(p, sw, StringComparison.OrdinalIgnoreCase)));
            var directors = searchWords.Where(sw => favoriteDirectors.Any(p => string.Equals(p, sw, StringComparison.OrdinalIgnoreCase)));

            var actorFilter = Builders<MovieMdo>.Filter.AnyIn(f => f.Actors, actors);
            var directorFilter = Builders<MovieMdo>.Filter.In(f => f.Director, directors);
            var languageFilter = Builders<MovieMdo>.Filter.In(f => f.Language, favoriteLanguages);

            var filterDefinition = Builders<MovieMdo>.Filter.And(actorFilter, directorFilter, languageFilter);
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
    }
}
