using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Entities.Movie;
using Whatflix.Data.Abstract.Repository;
using Whatflix.Domain.Dto.Movie;

namespace Whatflix.Domain.Manage
{
    public class ManageMovie
    {
        private readonly IMovieRepository _moviesRepository;
        private readonly IUserPreferenceRepository _userPreferenceRepository;
        private IMapper _mapper;

        public ManageMovie(IMovieRepository moviesRepository,
            IUserPreferenceRepository userPreferenceRepository,
            IMapper mapper)
        {
            _moviesRepository = moviesRepository;
            _userPreferenceRepository = userPreferenceRepository;
            _mapper = mapper;
        }

        public async Task InsertMany(IEnumerable<MovieDto> movieDtos)
        {
            await _moviesRepository.InsertMany(_mapper.Map<IEnumerable<IMovie>>(movieDtos));
        }

        public async Task<List<MovieDto>> SearchAsync(string[] searchWords, int userId)
        {
            var userPreference = await _userPreferenceRepository.GetMovieIdsByUserIdAsync(userId);
            var movieObjects = await _moviesRepository.SearchAsync(searchWords, userPreference);

            return _mapper.Map<List<MovieDto>>(movieObjects);
        }

        public async Task<List<MovieDto>> SearchAsync(string[] searchWords)
        {
            var movieObjects = await _moviesRepository.SearchAsync(searchWords);
            return _mapper.Map<List<MovieDto>>(movieObjects);
        }
    }
}
