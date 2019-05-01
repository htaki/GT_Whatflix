using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Entities.Movie;
using Whatflix.Data.Abstract.Repository;
using Whatflix.Domain.Abstract.Dto.Movie;
using Whatflix.Domain.Abstract.Dto.UserPreference;
using Whatflix.Domain.Abstract.Manage;
using Whatflix.Domain.Dto.UserPreference;

namespace Whatflix.Domain.Manage
{
    public class Movie : IMovie
    {
        private readonly IMovieRepository _moviesRepository;
        private IMapper _mapper;

        public Movie(IMovieRepository moviesRepository,
            IMapper mapper)
        {
            _moviesRepository = moviesRepository;
            _mapper = mapper;
        }

        public async Task InsertMany(IEnumerable<IMovieDto> movieDtos)
        {
            if (movieDtos == null)
            {
                throw new ArgumentNullException(nameof(movieDtos));
            }

            if (!movieDtos.Any())
            {
                throw new ArgumentException($"{nameof(movieDtos)} cannot be empty.");
            }

            await _moviesRepository.InsertMany(_mapper.Map<IEnumerable<IMovieEntity>>(movieDtos));
        }

        public async Task<List<IMovieDto>> SearchAsync(string[] searchWords, IUserPreferenceDto userPreference)
        {
            if (searchWords == null)
            {
                throw new ArgumentNullException(nameof(searchWords));
            }

            if (!searchWords.Any())
            {
                throw new ArgumentException($"{nameof(searchWords)} cannot be empty.");
            }

            if (userPreference == null)
            {
                throw new ArgumentNullException(nameof(userPreference));
            }

            var movieObjects = await _moviesRepository.SearchAsync(searchWords,
                userPreference.FavoriteActors,
                userPreference.FavoriteDirectors,
                userPreference.PreferredLanguages);

            return _mapper.Map<List<IMovieDto>>(movieObjects);
        }

        public async Task<List<IMovieDto>> SearchAsync(string[] searchWords)
        {
            if (searchWords == null)
            {
                throw new ArgumentNullException(nameof(searchWords));
            }

            if (!searchWords.Any())
            {
                throw new ArgumentException($"{nameof(searchWords)} cannot be empty.");
            }

            var movieObjects = await _moviesRepository.SearchAsync(searchWords);
            return _mapper.Map<List<IMovieDto>>(movieObjects);
        }

        public async Task UpdateAppeardInSearchAsync(List<int> movieIds)
        {
            if (movieIds == null)
            {
                throw new ArgumentNullException(nameof(movieIds));
            }

            await _moviesRepository.UpdateAppeardInSearchAsync(movieIds);
        }

        public async Task<List<IRecommendationsDto>> GetRecommendationsAsync(IEnumerable<IUserPreferenceDto> userPreferences)
        {
            if (userPreferences == null)
            {
                throw new ArgumentNullException(nameof(userPreferences));
            }

            if (!userPreferences.Any())
            {
                throw new ArgumentException($"{nameof(userPreferences)} cannot be empty.");
            }

            var recommendations = new List<IRecommendationsDto>();

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
