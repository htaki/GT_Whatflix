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
    public class MoviesElasticsearchRepository : BaseElasticsearchRepository<IMovieEntity, MovieAdo>, IMovieRepository
    {
        private readonly IMapper _mapper;
        private const string INDEX_ALIAS_MOVIES = "whatflix-movies";

        public MoviesElasticsearchRepository(IOptions<SettingsWrapper> serviceSettings, IMapper mapper) : base(serviceSettings, INDEX_ALIAS_MOVIES, mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<IMovieEntity>> SearchAsync(string[] searchWords)
        {
            var multiMatchContainer = new QueryContainer();

            foreach (var searchWord in searchWords)
            {
                multiMatchContainer |= new QueryContainerDescriptor<MovieAdo>().MultiMatch(m => m
                    .Query(searchWord)
                    .Fields(fs => fs
                        .Field(f => f.Director.Suffix("keyword"))
                        .Field(f => f.Title.Suffix("keyword"))
                        .Field(f => f.Actors.Suffix("keyword"))
                    )
                );
            }

            var searchResponse = await _client.SearchAsync<MovieAdo>(s => s
                .Query(query => multiMatchContainer)
                .Sort(so => so
                    .Ascending(f => f.Title.Suffix("keyword"))
                )
                .Source(source => source
                    .Includes(i => i
                        .Field(f => f.Title)
                        .Field(f => f.MovieId)
                    )
                )
                .Size(int.MaxValue)
            );

            var doucments = searchResponse.Documents;
            return _mapper.Map<List<IMovieEntity>>(doucments);
        }

        public async Task<List<IMovieEntity>> SearchAsync(string[] searchWords, 
            List<string> favoriteActors, 
            List<string> favoriteDirectors, 
            List<string> favoriteLanguages)
        {
            var searchWordsContainer = new QueryContainer();

            foreach (var searchWord in searchWords)
            {
                searchWordsContainer |= new QueryContainerDescriptor<MovieAdo>().MultiMatch(m => m
                    .Query(searchWord)
                    .Fields(fs => fs
                        .Field(f => f.Director.Suffix("keyword"))
                        .Field(f => f.Title.Suffix("keyword"))
                        .Field(f => f.Actors.Suffix("keyword"))
                    )
                );
            }

            QueryContainer searchQuery(QueryContainerDescriptor<MovieAdo> query) => query
                .Bool(b => b
                        .Must(m_ => m_
                            .Bool(b2 => b2
                                .Should(s => s
                                    .Terms(t => t
                                        .Terms(favoriteActors)
                                        .Field(f => f.Actors.Suffix("keyword"))
                                    ), s => s
                                    .Terms(t => t
                                        .Terms(favoriteDirectors)
                                        .Field(f => f.Director.Suffix("keyword"))
                                    )
                                )
                                .MinimumShouldMatch(MinimumShouldMatch.Fixed(1))
                            ), m1 => m1
                            .Bool(b1 => b1
                                .Should(m2 => m2
                                    .Terms(t => t
                                        .Terms(favoriteLanguages)
                                        .Field(f => f.Language.Suffix("keyword"))
                                    )
                                )
                                .MinimumShouldMatch(MinimumShouldMatch.Fixed(1))
                            ), m1 => m1
                        )
                        .Filter(searchWordsContainer)
                    );

            var debugQuery = GetRawQueryForDebug(new SearchDescriptor<MovieAdo>().Query(searchQuery));

            var searchResponse = await _client.SearchAsync<MovieAdo>(s => s
            .Query(searchQuery)
                .Sort(so => so
                    .Ascending(f => f.Title.Suffix("keyword"))
                )
                .Source(source => source
                    .Includes(i => i
                        .Field(f => f.Title)
                        .Field(f => f.MovieId)
                    )
                )
                .Size(int.MaxValue)
            );

            var doucments = searchResponse.Documents;
            return _mapper.Map<List<IMovieEntity>>(doucments);
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
                    .Source($"ctx._source.appearedInSearches = ctx._source.appearedInSearches + 1;")
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
                    .Descending(f => f.AppearedInSearches)
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

        private string GetSearchQuery(List<string> searchWords)
        {
            var searchQuery = "";

            for (int i = 0; i < searchWords.Count; i++)
            {
                searchQuery += "\"" + searchWords[i] + "\"";

                if (i < searchWords.Count - 1)
                {
                    searchQuery += " ";
                }
            }

            return searchQuery;
        }
    }
}
