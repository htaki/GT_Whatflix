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
    public class MoviesMongoRepositoryTest
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

        #region Private Methods

        private IOptions<SettingsWrapper> CreateSettings()
        {
            return Options.Create(new SettingsWrapper
            {
                Databases = new Databases
                {
                    Elasticsearch = new Database
                    {
                        ConnectionString = "https://vsq4ajXp6Y:NasvTBMfGWreE4g9Ut3XA75@sandbox-cluster-9944218666.ap-southeast-2.bonsaisearch.net"
                    },
                    MongoDb = new Database
                    {
                        ConnectionString = "mongodb://admin:admin@sandbox-cluster-shard-00-00-a38l5.mongodb.net:27017,sandbox-cluster-shard-00-01-a38l5.mongodb.net:27017,sandbox-cluster-shard-00-02-a38l5.mongodb.net:27017/whatflix?ssl=true&replicaSet=sandbox-cluster-shard-0&authSource=admin&retryWrites=true"
                    }
                }
            });
        }

        #endregion
    }
}
