using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Entities.UserPreference;
using Whatflix.Data.Abstract.Repository;
using Whatflix.Domain.Dto.UserPreference;

namespace Whatflix.Domain.Manage
{
    public class ManageUserPreference
    {
        private readonly IUserPreferenceRepository _userPreferenceRepository;
        private IMapper _mapper;

        public ManageUserPreference(IUserPreferenceRepository userPreferenceRepository,
            IMapper mapper)
        {
            _userPreferenceRepository = userPreferenceRepository;
            _mapper = mapper;
        }

        public async Task InsertMany(IEnumerable<UserPreferenceDto> userPreferenceDtos)
        {
            var userPreferences = _mapper.Map<IEnumerable<IUserPreference>>(userPreferenceDtos);
            await _userPreferenceRepository.InsertMany(userPreferences);
        }
    }
}
