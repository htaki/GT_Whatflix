using Microsoft.Extensions.Options;
using Whatflix.Api._03_Data.Mdo;

namespace Whatflix.Api._03_Data.Repository
{
    public class MoviesRepository : BaseRepository<MovieMdo>
    {
        public const string COLLECTION_NAME = "movies";

        public MoviesRepository(IOptions<ServiceSettings> serviceSettings)
            : base(serviceSettings.Value.Whatfix_Db.ConnectionString, COLLECTION_NAME)
        {
        }
    }
}
