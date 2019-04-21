using Nest;
using Whatflix.Data.Elasticsearch.Ado.Movie;

namespace Whatflix.Data.Elasticsearch
{
    public class AnalyzedMapping
    {
        public static CreateIndexDescriptor IndexDescriptor(CreateIndexDescriptor createIndexDescriptor)
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
                                )
                            )
                            .Text(txt => txt
                                .Name(name => name.Language)
                                .Fields(f => f
                                    .Text(f_txt => f_txt
                                        .Name("search")
                                        .Analyzer("text_analyzer")
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
                                )
                            )
                            .Text(txt => txt
                                .Name(name => name.Actors)
                                .Fields(f => f
                                    .Text(f_txt => f_txt
                                        .Name("search")
                                        .Analyzer("text_analyzer")
                                    )
                                )
                            )
                        )
                    )
                );
        }
    }
}
