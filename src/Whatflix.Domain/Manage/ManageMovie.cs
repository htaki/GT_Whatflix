using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Entities.Movie;
using Whatflix.Data.Abstract.Repository;
using Whatflix.Domain.Dto.Movie;
using Whatflix.Domain.Dto.UserPreference;

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

        public async void UpdatedAppeardOnSearchAsync(List<int> movieIds)
        {
            await _moviesRepository.UpdatedAppeardInSearchAsync(movieIds);
        }

        public async Task<List<MovieDto>> SearchAsync(string[] searchWords)
        {
            var movieObjects = await _moviesRepository.SearchAsync(searchWords);
            return _mapper.Map<List<MovieDto>>(movieObjects);
        }

        public async Task<List<RecommendationsDto>> GetRecommendationsAsync()
        {
            var userPreferences = await _userPreferenceRepository.GetUserPreferences();
            var recommendations = new List<RecommendationsDto>();

            foreach (var userPreference in userPreferences)
            {
                var recommendedMovies = await _moviesRepository.GetRecommendationByMovieIdsAsync(userPreference.MovieIds);
                recommendations.Add(new RecommendationsDto
                {
                    Movies = recommendedMovies,
                    UserId = userPreference.UserId
                });
            }

            return recommendations;
        }
    }
}
