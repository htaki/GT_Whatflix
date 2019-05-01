using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Whatflix.Domain.Abstract.Dto.Movie;
using Whatflix.Domain.Abstract.Dto.UserPreference;
using Whatflix.Domain.Abstract.Manage;
using Whatflix.Domain.Dto.Movie;
using Whatflix.Presentation.Api.Controllers;
using Whatflix.Presentation.Api.Helpers;
using Whatflix.Presentation.Api.Models;

namespace Whatflix.Presentation.Api.Test.Controllers
{
    [TestClass]
    public class MoviesControllerTest
    {
        private MoviesController _moviesController;
        private Mock<IMovie> _mockMovie;
        private Mock<ControllerHelper> _mockControllerHelper;
        private Mock<IMapper> _mockMapper;

        [TestInitialize]
        public void InitializeTest()
        {
            _mockMovie = new Mock<IMovie>();
            _mockControllerHelper = new Mock<ControllerHelper>();
            _mockMapper = new Mock<IMapper>();
            _moviesController = new MoviesController(_mockMovie.Object, _mockControllerHelper.Object, _mockMapper.Object);
        }

        [TestMethod]
        public void Get_InvalidUserId_ValidSearchText_ReturnsBadRequest()
        {
            // Arrange
            var expectedErrorMessage = "The userId is not valid.";
            var expectedStatusCode = StatusCodes.Status400BadRequest;

            // Act
            var actualResult = _moviesController.Get(It.IsAny<int>(), "Steven Spielberg").Result;

            // Assert
            Assert.IsInstanceOfType(actualResult, typeof(ObjectResult));
            Assert.AreEqual(((ObjectResult)actualResult).Value, expectedErrorMessage);
            Assert.AreEqual(((ObjectResult)actualResult).StatusCode, expectedStatusCode);
        }

        [TestMethod]
        public void Get_ValidUserId_InvalidSearchText_ReturnsBadRequest()
        {
            // Arrange
            var expectedErrorMessage = "Search text cannot be empty.";
            var expectedStatusCode = StatusCodes.Status400BadRequest;

            // Act
            var actualResult = _moviesController.Get(100, It.IsAny<string>()).Result;

            // Assert
            Assert.IsInstanceOfType(actualResult, typeof(ObjectResult));
            Assert.AreEqual(((ObjectResult)actualResult).Value, expectedErrorMessage);
            Assert.AreEqual(((ObjectResult)actualResult).StatusCode, expectedStatusCode);
        }

        [TestMethod]
        public void Get_ValidUserId_ValidSearchText_ReturnsOk()
        {
            // Arrange
            _mockMovie.Setup(s => s.SearchAsync(It.IsAny<string[]>())).ReturnsAsync(MockMovieDtos);
            _mockMovie.Setup(s => s.SearchAsync(It.IsAny<string[]>(), It.IsAny<IUserPreferenceDto>())).ReturnsAsync(MockMovieDtos);
            _mockMovie.Setup(s => s.UpdateAppeardInSearchAsync(new List<int> { 1, 2, 3 }));
            _mockControllerHelper.Setup(s => s.GetUserPreferencesByUserId(100)).Returns(MockUserPreferenceModel);
            var expectedStatusCode = StatusCodes.Status200OK;

            // Act
            var actualResult = _moviesController.Get(100, "Steven Spielberg").Result;

            // Assert
            Assert.IsInstanceOfType(actualResult, typeof(ObjectResult));
            var result = (ObjectResult)actualResult;
            Assert.AreEqual(result.StatusCode, expectedStatusCode);
            Assert.AreEqual((result.Value as IEnumerable<string>).ElementAt(0), "The Equalizer");
        }

        private List<IMovieDto> MockMovieDtos()
        {
            return new List<IMovieDto>
            {
                new MovieDto
                {
                    Actors = new List<string>{ "Denzel Washington" },
                    Director = "Antoine Fuqua",
                    Language = "English",
                    MovieId = 1,
                    Title = "The Equalizer"
                }
            };
        }

        private UserPreferenceModel MockUserPreferenceModel()
        {
            return new UserPreferenceModel
            {
                FavoriteDirectors = new List<string> { "Steven Spielberg", "Martin Scorsese", "Pedro Almodóvar" },
                FavoriteActors = new List<string> { "Denzel Washington", "Kate Winslet", "Emma Suárez", "Tom Hanks" },
                PreferredLanguages = new List<string> { "English", "Spanish" },
                UserId = 100
            };
        }
    }
}
