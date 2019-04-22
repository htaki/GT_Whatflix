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
                .Source(source => source
                    .Includes(i => i
                        .Field(f => f.Title)
                    )
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
                .Source(source => source
                    .Includes(i => i
                        .Field(f => f.Title)
                    )
                )
                .Size(int.MaxValue)
            );

            var doucments = searchResponse.Documents;
            return _mapper.Map<List<IMovie>>(doucments);
        }

        public async Task UpdatedAppeardInSearchAsync(List<int> movieIds)
        {
            await _client.UpdateByQueryAsync<MovieAdo>(u => u.Query(q => q
                    .Terms(t => t
                        .Field(f => f.MovieId)
                        .Terms(movieIds)
                    )
                 )
                 .Script(script => script
                    .Source($"ctx._source.AppearedInSearches = ctx._source.AppearedInSearches + 1;")
                 )
             );
        }

        public async Task<IEnumerable<string>> GetRecommendationByMovieIdsAsync(List<int> movieIds)
        {
            var searchResponse = await _client.SearchAsync<MovieAdo>(q => q
                .Query(query => query
                    .Terms(t => t
                        .Field(f => f.MovieId)
                        .Terms(movieIds)
                    )
                )
                .Sort(so => so
                    .Ascending(f => f.AppearedInSearches)
                )
                .Source(source => source
                    .Includes(i => i
                        .Field(f => f.Title)
                    )
                )
                .Size(3)
            );

            var doucments = searchResponse.Documents;
            return doucments.Select(d => d.Title);
        }

        private string GetRawQueryForDebug(SearchDescriptor<MovieAdo> query)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                _client.RequestResponseSerializer.Serialize(query, memoryStream);
                return Encoding.ASCII.GetString(memoryStream.ToArray());
            }
        }
    }
}
