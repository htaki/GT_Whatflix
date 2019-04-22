using AutoMapper;
using Microsoft.Extensions.Options;
using Nest;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Entities.Movie;
using Whatflix.Data.Abstract.Repository;
using Whatflix.Data.Elasticsearch.Ado.Movie;
using Whatflix.Infrastructure.ServiceSettings;

namespace Whatflix.Data.Elasticsearch.Repository
{
    public class MoviesElasticsearchRepository : BaseElasticsearchRepository<IMovie, MovieAdo>, IMovieRepository
    {
        private readonly IMapper _mapper;
        private const string INDEX_ALIAS_MOVIES = "whatflix-movies";

        public MoviesElasticsearchRepository(IOptions<SettingsWrapper> serviceSettings, IMapper mapper) : base(serviceSettings, INDEX_ALIAS_MOVIES, mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<IMovie>> SearchAsync(string[] searchWords)
        {
            var searchResponse = await _client.SearchAsync<MovieAdo>(s => s
                .Query(query => query
                    .Bool(b => b
                        .Should(sh => sh
                            .Terms(t => t
                                .Terms(searchWords)
                                .Field(f => f.Actors.Suffix("keyword"))
                            ), sh => sh
                            .Terms(t => t
                                .Terms(searchWords)
                                .Field(f => f.Director.Suffix("keyword"))
                            ), sh => sh
                            .Terms(t => t
                                .Terms(searchWords)
                                .Field(f => f.Title.Suffix("keyword"))
                            )
                        )
                    )
                )
                .Sort(so => so
                    .Ascending(f => f.Title.Suffix("keyword"))
                )
                .Size(int.MaxValue)
            );

            var doucments = searchResponse.Documents;
            return _mapper.Map<List<IMovie>>(doucments);
        }

        public async Task<List<IMovie>> SearchAsync(string[] searchWords, List<int> movieIds)
        {
            var searchResponse = await _client.SearchAsync<MovieAdo>(s => s
                .Query(query => query
                    .Bool(b => b
                        .Should(m => m
                            .Terms(t => t
                                .Terms(searchWords)
                                .Field(f => f.Actors.Suffix("keyword"))
                            ), m => m
                            .Terms(t => t
                                .Terms(searchWords)
                                .Field(f => f.Director.Suffix("keyword"))
                            ), m => m
                            .Terms(t => t
                                .Terms(searchWords)
                                .Field(f => f.Language.Suffix("keyword"))
                            )
                        )
                        .Must(m => m
                            .Terms(t => t
                                .Terms(movieIds)
                                .Field(f => f.MovieId)
                            )
                        )
                    )
                )
                .Sort(so => so
                    .Ascending(f => f.Title.Suffix("keyword"))
                )
                .Size(int.MaxValue)
            );

            var doucments = searchResponse.Documents;
            return _mapper.Map<List<IMovie>>(doucments);
        }

        private IEnumerable<object> Resolve(IEnumerable<string> preferredActors)
        {
            if (!preferredActors.Any())
            {
                return new string[] { "null!" };
            }

            return preferredActors;
        }

        private string RawQuery(SearchDescriptor<MovieAdo> debugQuery)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                _client.RequestResponseSerializer.Serialize(debugQuery, mStream);
                return Encoding.ASCII.GetString(mStream.ToArray());
            }
        }
    }
}
