using System.Collections.Generic;

namespace Whatflix.Api._03_Data.Mdo
{
    public class UserPreferenceMdo : BaseMdo
    {
        public int UserId { get; set; }
        public List<MovieMdo> Movies { get; set; }
    }
}
