using AutoMapper;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
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
            //var searchResponse = await _client.SearchAsync<MovieAdo>(s => s
            //    .Query(query => query
            //        .Bool(b => b
            //            .Should(m => m
            //                .MultiMatch(mm => mm
            //                    .Fields(fs => fs
            //                        .Field(f => f.Actors.Suffix("search"))
            //                        .Field(f => f.Director.Suffix("search"))
            //                        .Field(f => f.Title.Suffix("search"))
            //                    )
            //                    .Query(string.Join(" ", searchWords))
            //                )
            //            )
            //        )
            //    )
            //);

            var searchQuery = "(";
            for (int i = 0; i < searchWords.Length; i++)
            {
                searchQuery += searchWords[i] + ")";

                if (i < searchWords.Length - 1)
                {
                    searchQuery += " OR ";
                }
            }

            var searchResponse = await _client.SearchAsync<MovieAdo>(s => s
                .Query(query => query
                    .QueryString(qs => qs
                        .Query(searchQuery)
                        .Fields(fs => fs
                            .Field(f => f.Actors.Suffix("search"))
                            .Field(f => f.Director.Suffix("search"))
                            .Field(f => f.Title.Suffix("search"))
                        )
                    )
                )
            );

            var doucments = searchResponse.Documents;

            return _mapper.Map<List<IMovie>>(doucments);
        }

        public async Task<List<IMovie>> Search(string[] searchWords, string[] favoriteActors, string[] favoriteDirectors, string[] favoriteLanguages)
        {
            var actors = searchWords.Where(sw => favoriteActors.Any(p => string.Equals(p, sw, StringComparison.OrdinalIgnoreCase)));
            var directors = searchWords.Where(sw => favoriteDirectors.Any(p => string.Equals(p, sw, StringComparison.OrdinalIgnoreCase)));

            var searchResponse = await _client.SearchAsync<MovieAdo>(s => s
                .Query(query => query
                    .Bool(b => b
                        .Must(m => m
                            .Bool(b_ => b_
                                .Should(sh => sh
                                    .Match(ma => ma
                                        .Field(f => f.Actors.Suffix("search"))
                                        .Query(string.Join(" ", actors))
                                    )
                                )
                            )
                        )
                    )
                )
            );

            var doucments = searchResponse.Documents;

            return _mapper.Map<List<IMovie>>(doucments);
        }
    }
}
