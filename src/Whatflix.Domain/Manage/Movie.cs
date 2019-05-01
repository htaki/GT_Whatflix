using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Entities.Movie;
using Whatflix.Data.Abstract.Repository;
using Whatflix.Domain.Dto.Movie;
using Whatflix.Domain.Dto.UserPreference;

namespace Whatflix.Domain.Manage
{
    public class Movie
    {
        private readonly IMovieRepository _moviesRepository;
        private IMapper _mapper;

        public Movie(IMovieRepository moviesRepository,
            IMapper mapper)
        {
            _moviesRepository = moviesRepository;
            _mapper = mapper;
        }

        public async Task InsertMany(IEnumerable<MovieDto> movieDtos)
        {
            await _moviesRepository.InsertMany(_mapper.Map<IEnumerable<IMovieEntity>>(movieDtos));
        }

        public async Task<List<MovieDto>> SearchAsync(string[] searchWords, UserPreferenceDto userPreference)
        {
            var movieObjects = await _moviesRepository.SearchAsync(searchWords,
                userPreference.FavoriteActors, userPreference.FavoriteDirectors, userPreference.PreferredLanguages);
            return _mapper.Map<List<MovieDto>>(movieObjects);
        }

        public async void UpdatedAppeardInSearchAsync(List<int> movieIds)
        {
            await _moviesRepository.UpdatedAppeardInSearchAsync(movieIds);
        }

        public async Task<List<MovieDto>> SearchAsync(string[] searchWords)
        {
            var movieObjects = await _moviesRepository.SearchAsync(searchWords);
            return _mapper.Map<List<MovieDto>>(movieObjects);
        }

        public async Task<List<RecommendationsDto>> GetRecommendationsAsync(IEnumerable<UserPreferenceDto> userPreferences)
        {
            var recommendations = new List<RecommendationsDto>();

            foreach (var userPreference in userPreferences)
            {
                var recommendedMovies = await _moviesRepository.GetRecommendationsAsync(userPreference.FavoriteActors, userPreference.FavoriteDirectors, userPreference.PreferredLanguages);
                recommendations.Add(new RecommendationsDto
                {
                    Movies = recommendedMovies.Select(r => r.Title).OrderBy(o => o),
                    User = userPreference.UserId
                });
            }

            return recommendations;
        }
    }
}
