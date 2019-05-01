using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Repository;
using Whatflix.Domain.Abstract.Dto.Movie;
using Whatflix.Domain.Abstract.Dto.UserPreference;
using Whatflix.Domain.Dto.UserPreference;
using Whatflix.Domain.Manage;

namespace Whatflix.Domain.Test.Manage
{
    [TestClass]
    public class MovieTest
    {
        private Movie _movie;
        private Mock<IMovieRepository> _mockMovieRepository;
        private Mock<IMapper> _mockMapper;

        [TestInitialize]
        public void InitializeTest()
        {
            _mockMovieRepository = new Mock<IMovieRepository>();
            _mockMapper = new Mock<IMapper>();
            _movie = new Movie(_mockMovieRepository.Object, _mockMapper.Object);
        }

        [TestMethod]
        public async Task InsertMany_InvalidParameter_ThrowsArgumentNullException()
        {
            // Arrange
            var expectedErrorType = typeof(ArgumentNullException);
            var expectedMessage = "Value cannot be null.\r\nParameter name: movieDtos";

            try
            {
                // Act
                await _movie.InsertMany(It.IsAny<IEnumerable<IMovieDto>>());

                // Assert
                Assert.Fail();
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsInstanceOfType(ex, expectedErrorType);
                Assert.AreEqual(expectedMessage, ex.Message);
            }
        }

        [TestMethod]
        public async Task InsertMany_EmptyParameter_ThrowsArgumentException()
        {
            // Arrange
            var expectedErrorType = typeof(ArgumentException);
            var expectedMessage = "movieDtos cannot be empty.";

            try
            {
                // Act
                await _movie.InsertMany(new List<IMovieDto> { });

                // Assert
                Assert.Fail();
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsInstanceOfType(ex, expectedErrorType);
                Assert.AreEqual(expectedMessage, ex.Message);
            }
        }

        [TestMethod]
        public async Task SearchAsync_InvalidSearchWords_ThrowsArgumentNullException()
        {
            // Arrange
            var expectedErrorType = typeof(ArgumentNullException);
            var expectedMessage = "Value cannot be null.\r\nParameter name: searchWords";

            try
            {
                // Act
                await _movie.SearchAsync(It.IsAny<string[]>(), new UserPreferenceDto());

                // Assert
                Assert.Fail();
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsInstanceOfType(ex, expectedErrorType);
                Assert.AreEqual(expectedMessage, ex.Message);
            }
        }

        [TestMethod]
        public async Task SearchAsync_EmptySearchWords_ThrowsArgumentException()
        {
            // Arrange
            var expectedErrorType = typeof(ArgumentException);
            var expectedMessage = "searchWords cannot be empty.";

            try
            {
                // Act
                await _movie.SearchAsync(new string[] { }, new UserPreferenceDto());

                // Assert
                Assert.Fail();
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsInstanceOfType(ex, expectedErrorType);
                Assert.AreEqual(expectedMessage, ex.Message);
            }
        }

        [TestMethod]
        public async Task SearchAsync_InvalidParameter_ThrowsArgumentNullException()
        {
            // Arrange
            var expectedErrorType = typeof(ArgumentNullException);
            var expectedMessage = "Value cannot be null.\r\nParameter name: searchWords";

            try
            {
                // Act
                await _movie.SearchAsync(It.IsAny<string[]>());

                // Assert
                Assert.Fail();
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsInstanceOfType(ex, expectedErrorType);
                Assert.AreEqual(expectedMessage, ex.Message);
            }
        }

        [TestMethod]
        public async Task SearchAsync_EmptyParameter_ThrowsArgumentException()
        {
            // Arrange
            var expectedErrorType = typeof(ArgumentException);
            var expectedMessage = "searchWords cannot be empty.";

            try
            {
                // Act
                await _movie.SearchAsync(new string[] { });

                // Assert
                Assert.Fail();
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsInstanceOfType(ex, expectedErrorType);
                Assert.AreEqual(expectedMessage, ex.Message);
            }
        }

        [TestMethod]
        public async Task UpdateAppeardInSearchAsync_InvalidParameter_ThrowsArgumentNullException()
        {
            // Arrange
            var expectedErrorType = typeof(ArgumentNullException);
            var expectedMessage = "Value cannot be null.\r\nParameter name: movieIds";

            try
            {
                // Act
                await _movie.UpdateAppeardInSearchAsync(It.IsAny<List<int>>());

                // Assert
                Assert.Fail();
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsInstanceOfType(ex, expectedErrorType);
                Assert.AreEqual(expectedMessage, ex.Message);
            }
        }

        [TestMethod]
        public async Task GetRecommendationsAsync_InvalidParameter_ThrowsArgumentNullException()
        {
            // Arrange
            var expectedErrorType = typeof(ArgumentNullException);
            var expectedMessage = "Value cannot be null.\r\nParameter name: userPreferences";

            try
            {
                // Act
                await _movie.GetRecommendationsAsync(It.IsAny<IEnumerable<IUserPreferenceDto>>());

                // Assert
                Assert.Fail();
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsInstanceOfType(ex, expectedErrorType);
                Assert.AreEqual(expectedMessage, ex.Message);
            }
        }

        [TestMethod]
        public async Task GetRecommendationsAsync_EmptyParameter_ThrowsArgumentException()
        {
            // Arrange
            var expectedErrorType = typeof(ArgumentException);
            var expectedMessage = "userPreferences cannot be empty.";

            try
            {
                // Act
                await _movie.GetRecommendationsAsync(new List<IUserPreferenceDto> { });

                // Assert
                Assert.Fail();
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsInstanceOfType(ex, expectedErrorType);
                Assert.AreEqual(expectedMessage, ex.Message);
            }
        }
    }
}
