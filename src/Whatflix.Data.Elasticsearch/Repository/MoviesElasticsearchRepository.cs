using AutoMapper;
using Microsoft.Extensions.Options;
using Nest;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Entities.Movie;
using Whatflix.Data.Abstract.Repository;
using Whatflix.Data.Elasticsearch.Ado.Movie;
using Whatflix.Infrastructure.Helpers.Constants;
using Whatflix.Infrastructure.ServiceSettings;

namespace Whatflix.Data.Elasticsearch.Repository
{
    public class MoviesElasticsearchRepository : BaseElasticsearchRepository<IMovieEntity, MovieAdo>, IMovieRepository
    {
        private readonly IMapper _mapper;

        public MoviesElasticsearchRepository(IOptions<SettingsWrapper> serviceSettings, IMapper mapper) : base(serviceSettings, WhatflixConstants.DATABASE_NAME, mapper)
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
            List<string> preferredLanguages)
        {

            QueryContainer searchQuery(QueryContainerDescriptor<MovieAdo> query) => query
                .Bool(b => b
                    .Should(s => s
                        .Terms(t => t
                            .Field(f => f
                                .Director.Suffix("keyword")
                            )
                            .Terms(favoriteDirectors)
                        ), s => s
                        .Terms(t => t
                            .Field(f => f
                                .Actors.Suffix("keyword")
                            )
                            .Terms(favoriteActors)
                        )
                    )
                    .MinimumShouldMatch(1)
                    .Must(m => m
                        .Terms(t => t
                            .Field(f => f
                                .Language.Suffix("keyword")
                            )
                            .Terms(preferredLanguages)
                        )
                    )
                    .Filter(fi =>
                    {
                        var queryContainer = new QueryContainer();

                        foreach (var searchWord in searchWords)
                        {
                            queryContainer |= new QueryContainerDescriptor<MovieAdo>().MultiMatch(m => m
                                .Query(searchWord)
                                .Fields(fs => fs
                                    .Field(f => f.Director.Suffix("keyword"))
                                    .Field(f => f.Title.Suffix("keyword"))
                                    .Field(f => f.Actors.Suffix("keyword"))
                                )
                            );
                        }

                        return queryContainer;
                    })
                );

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

        public async Task UpdateAppeardInSearchAsync(List<int> movieIds)
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

        public async Task<List<IMovieEntity>> GetRecommendationsAsync(List<string> favoriteActors,
            List<string> favoriteDirectors,
            List<string> preferredLanguages)
        {
            QueryContainer searchQuery(QueryContainerDescriptor<MovieAdo> query) => query
              .Bool(b => b
                  .Should(s => s
                      .Terms(t => t
                          .Field(f => f
                              .Director.Suffix("keyword")
                          )
                          .Terms(favoriteDirectors)
                      ), s => s
                      .Terms(t => t
                          .Field(f => f
                              .Actors.Suffix("keyword")
                          )
                          .Terms(favoriteActors)
                      )
                  )
                  .MinimumShouldMatch(1)
                  .Must(m => m
                      .Terms(t => t
                          .Field(f => f
                              .Language.Suffix("keyword")
                          )
                          .Terms(preferredLanguages)
                      )
                  )
              );

            var searchResponse = await _client.SearchAsync<MovieAdo>(s => s
            .Query(searchQuery)
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
            return _mapper.Map<List<IMovieEntity>>(doucments);
        }

        private string GetRawQueryForDebug(SearchDescriptor<MovieAdo> query)
        {
            // usage:
            // var debugQuery = GetRawQueryForDebug(new SearchDescriptor<MovieAdo>().Query(searchQuery));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                _client.RequestResponseSerializer.Serialize(query, memoryStream);
                return Encoding.ASCII.GetString(memoryStream.ToArray());
            }
        }
    }
}
