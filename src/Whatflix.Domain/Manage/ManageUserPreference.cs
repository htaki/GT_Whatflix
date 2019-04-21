using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Whatflix.Domain.Dto.UserPreference;

namespace Whatflix.Domain.Manage
{
    public sealed class ManageUserPreference
    {
        private const string PATH = "";
        private static ManageUserPreference _instance;
        private static readonly object padlock = new object();
        private IEnumerable<UserPreferenceDto> _userPreferences;

        ManageUserPreference()
        {
        }

        public static ManageUserPreference Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new ManageUserPreference();

                    }

                    return _instance;
                }
            }
        }

        public async Task<UserPreferenceDto> GetUserPreferenceById(int userId)
        {
            if(_userPreferences == null)
            {
                using (StreamReader streamReader = new StreamReader(PATH))
                {
                    var conetent = await streamReader.ReadToEndAsync();
                    _userPreferences = JsonConvert.DeserializeObject<List<UserPreferenceDto>>(conetent);
                }
            }

            return _userPreferences.FirstOrDefault(u => u.UserId == userId);
        }
    }
}
