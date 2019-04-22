using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Entities.UserPreference;
using Whatflix.Data.Abstract.Repository;
using Whatflix.Domain.Dto.UserPreference;

namespace Whatflix.Domain.Manage
{
    public class UserPreference
    {
        private readonly IUserPreferenceRepository _userPreferenceRepository;
        private IMapper _mapper;

        public UserPreference(IUserPreferenceRepository userPreferenceRepository,
            IMapper mapper)
        {
            _userPreferenceRepository = userPreferenceRepository;
            _mapper = mapper;
        }

        public async Task InsertMany(IEnumerable<UserPreferenceDto> userPreferenceDtos)
        {
            var userPreferences = _mapper.Map<IEnumerable<IUserPreferenceEntity>>(userPreferenceDtos);
            await _userPreferenceRepository.InsertMany(userPreferences);
        }
    }
}
