using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Repository;
using Whatflix.Domain.Abstract.Dto.Movie;
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
    }
}
