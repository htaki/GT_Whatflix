﻿using AutoMapper;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
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
            if (searchWords == null)
            {
                throw new ArgumentNullException(nameof(searchWords));
            }

            var searchResponse = await _client.SearchAsync<MovieAdo>(s => s
                .Query(query => GetSearchWordsQuery(searchWords))
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
            if (searchWords == null)
            {
                throw new ArgumentNullException(nameof(searchWords));
            }

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
                    .Filter(fi => GetSearchWordsQuery(searchWords))
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
            if (movieIds == null)
            {
                throw new ArgumentNullException(nameof(movieIds));
            }

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
            if (favoriteActors == null || favoriteDirectors == null || preferredLanguages == null)
            {
                throw new ArgumentException("User preference parameters are not valid.");
            }

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

        #region Private Methods

        private QueryContainer GetSearchWordsQuery(string[] searchWords)
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

            return multiMatchContainer;
        }

        #endregion
    }
}
