using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Entities.Movie;
using Whatflix.Data.Abstract.Repository;
using Whatflix.Data.Mongo.Repository;
using Whatflix.Infrastructure.ServiceSettings;

namespace Whatflix.Data.Mongo.Test.Repository
{
    [TestClass]
    public class MoviesMongoRepositoryTest : BaseRepositoryTest
    {
        private IMovieRepository _movieRepository;
        private IOptions<SettingsWrapper> _mockServieSettings;
        private Mock<IMapper> _mockMapper;


        [TestInitialize]
        public void InitializeTest()
        {
            _mockServieSettings = CreateSettings();
            _mockMapper = new Mock<IMapper>();
            _movieRepository = new MoviesMongoRepository(_mockServieSettings, _mockMapper.Object);

        }

        [TestMethod]
        public async Task InsertMany_InvalidParameter_ThrowsArgumentNullException()
        {
            // Arrange
            var expectedErrorType = typeof(ArgumentNullException);
            var expectedMessage = "Value cannot be null.\r\nParameter name: entities";

            try
            {
                // Act
                await _movieRepository.InsertMany(It.IsAny<IEnumerable<IMovieEntity>>());

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
        public async Task GetRecommendations_InvalidParameter_ThrowsArgumentNullException()
        {
            // Arrange
            var expectedErrorType = typeof(ArgumentNullException);
            var expectedMessage = "Value cannot be null.\r\nParameter name: values";

            try
            {
                // Act
                await _movieRepository.GetRecommendationsAsync(It.IsAny<List<string>>(), It.IsAny<List<string>>(), It.IsAny<List<string>>());

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
        public async Task Search_InvalidParameters_ThrowsArgumentNullException()
        {
            // Arrange
            var expectedErrorType = typeof(ArgumentNullException);
            var expectedMessage = "Value cannot be null.\r\nParameter name: searchWords";

            try
            {
                // Act
                await _movieRepository.SearchAsync(It.IsAny<string[]>(),It.IsAny<List<string>>(), It.IsAny<List<string>>(), It.IsAny<List<string>>());

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
        public async Task Search_InvalidSearchWords_ThrowsArgumentNullException()
        {
            // Arrange
            var expectedErrorType = typeof(ArgumentNullException);
            var expectedMessage = "Value cannot be null.\r\nParameter name: searchWords";

            try
            {
                // Act
                await _movieRepository.SearchAsync(It.IsAny<string[]>());

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
        public async Task Update_InvalidParameters_ThrowsArgumentNullException()
        {
            // Arrange
            var expectedErrorType = typeof(ArgumentNullException);
            var expectedMessage = "Value cannot be null.\r\nParameter name: movieIds";
            try
            {
                // Act
                await _movieRepository.UpdateAppeardInSearchAsync(It.IsAny<List<int>>());

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
