using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private IMapper _mapper;

        public ManageMovie(IMovieRepository moviesRepository,
            IMapper mapper)
        {
            _moviesRepository = moviesRepository;
            _mapper = mapper;
        }

        public async Task InsertMany(IEnumerable<MovieDto> movieDtos)
        {
            var movies = _mapper.Map<IEnumerable<IMovie>>(movieDtos);
            await _moviesRepository.InsertMany(movies);
        }

        public async Task<List<MovieDto>> Search(string[] searchWords, UserPreferenceDto userPreference)
        {
            var movieObjects = await _moviesRepository.Search(searchWords,
                GetPreferredActors(searchWords, userPreference),
                GetPreferredDirectors(searchWords, userPreference),
                userPreference.PreferredLanguages);

            return _mapper.Map<List<MovieDto>>(movieObjects);
        }

        public async Task<List<MovieDto>> Search(string[] searchWords)
        {
            var movieObjects = await _moviesRepository.Search(searchWords);
            return _mapper.Map<List<MovieDto>>(movieObjects);
        }

        private string[] GetPreferredActors(string[] searchWords, UserPreferenceDto userPreference)
        {
            return searchWords
                .Where(sw => userPreference.FavoriteActors
                .Any(p => string.Equals(p, sw, StringComparison.OrdinalIgnoreCase)))
                .ToArray();
        }

        private string[] GetPreferredDirectors(string[] searchWords, UserPreferenceDto userPreference)
        {
            return searchWords
                .Where(sw => userPreference.FavoriteDirectors
                .Any(p => string.Equals(p, sw, StringComparison.OrdinalIgnoreCase)))
                .ToArray();
        }
    }
}
