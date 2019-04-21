using AutoMapper;
using Nest;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Entities.Movie;
using Whatflix.Data.Abstract.Repository;
using Whatflix.Data.Elasticsearch.Ado.Movie;

namespace Whatflix.Data.Elasticsearch.Repository
{
    public class MoviesElasticsearchRepository : BaseElasticsearchRepository<IMovie, MovieAdo>, IMovieRepository
    {
        private readonly IMapper _mapper;

        public MoviesElasticsearchRepository(ElasticsearchWrapper elasticsearchWrapper, IMapper mapper) : base(elasticsearchWrapper, mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<IMovie>> Search(string[] searchWords)
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

        public async Task<List<IMovie>> Search(string[] searchWords, string[] preferredActors, string[] preferredDirectors, string[] favoriteLanguages)
        {
            var searchResponse = await _client.SearchAsync<MovieAdo>(s => s
                .Query(query => query
                    .Bool(b => b
                        .Must(m => m
                            .Terms(t => t
                                .Terms(Resolve(preferredActors))
                                .Field(f => f.Actors.Suffix("keyword"))
                            ), m => m
                            .Terms(t => t
                                .Terms(Resolve(preferredDirectors))
                                .Field(f => f.Director.Suffix("keyword"))
                            ), m => m
                            .Terms(t => t
                                .Terms(Resolve(favoriteLanguages))
                                .Field(f => f.Language.Suffix("keyword"))
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
