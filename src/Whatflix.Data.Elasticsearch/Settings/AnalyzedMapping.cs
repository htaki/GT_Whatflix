using Nest;
using Whatflix.Data.Elasticsearch.Ado.Movie;
using Whatflix.Data.Mongo.Ado.UserPreference;

namespace Whatflix.Data.Elasticsearch.Settings
{
    public class AnalyzedMapping
    {
        public static CreateIndexDescriptor MoviesIndexDescriptor(CreateIndexDescriptor createIndexDescriptor)
        {
            return createIndexDescriptor
                .Settings(settings => settings
                    .NumberOfShards(4)
                    .NumberOfReplicas(3)
                    .Setting("index.max_result_window", int.MaxValue)
                    .Analysis(analysis => analysis
                        .Analyzers(a => a
                            .Custom("text_analyzer", s => s
                                .Tokenizer("standard")
                                .Filters("lowercase", "asciifolding")
                            )
                        )
                    )
                )
                .Mappings(mappings => mappings
                    .Map<MovieAdo>(m => m
                        .Properties(p => p
                            .Text(txt => txt
                                .Name(name => name.Title)
                                .Fields(f => f
                                    .Text(f_txt => f_txt
                                        .Name("search")
                                        .Analyzer("text_analyzer")
                                    )
                                    .Keyword(kw => kw
                                        .Name("keyword")
                                    )
                                )
                            )
                            .Text(txt => txt
                                .Name(name => name.Language)
                                .Fields(f => f
                                    .Text(f_txt => f_txt
                                        .Name("search")
                                        .Analyzer("text_analyzer")
                                    )
                                    .Keyword(kw => kw
                                        .Name("keyword")
                                    )
                                )
                            )
                            .Text(txt => txt
                                .Name(name => name.Director)
                                .Fields(f => f
                                    .Text(f_txt => f_txt
                                        .Name("search")
                                        .Analyzer("text_analyzer")
                                    )
                                    .Keyword(kw => kw
                                        .Name("keyword")
                                    )
                                )
                            )
                            .Text(txt => txt
                                .Name(name => name.Actors)
                                .Fields(f => f
                                    .Text(f_txt => f_txt
                                        .Name("search")
                                        .Analyzer("text_analyzer")
                                    )
                                    .Keyword(kw => kw
                                        .Name("keyword")
                                    )
                                )
                            )
                        )
                    )
                );
        }

        public static CreateIndexDescriptor UserPreferencesIndexDescriptor(CreateIndexDescriptor createIndexDescriptor)
        {
            return createIndexDescriptor
                .Settings(settings => settings
                    .NumberOfShards(4)
                    .NumberOfReplicas(3)
                    .Setting("index.max_result_window", int.MaxValue)
                    .Analysis(analysis => analysis
                        .Analyzers(a => a
                            .Custom("text_analyzer", s => s
                                .Tokenizer("standard")
                                .Filters("lowercase", "asciifolding")
                            )
                        )
                    )
                )
                .Mappings(mappings => mappings
                    .Map<UserPreferenceAdo>(m => m
                        .AutoMap()
                    )
                );
        }
    }
}
