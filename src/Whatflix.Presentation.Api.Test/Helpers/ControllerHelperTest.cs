using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Whatflix.Presentation.Api.Helpers;
using Whatflix.Presentation.Api.Models;

namespace Whatflix.Presentation.Api.Test.Helpers
{
    [TestClass]
    public class ControllerHelperTest
    {
        private ControllerHelper _controllerHelper;

        [TestInitialize]
        public void InitializeTest()
        {
            _controllerHelper = new ControllerHelper();
        }

        [TestMethod]
        public void Get_ReturnsListOfMovies()
        {
            //Arrange 
            var expectedType = typeof(IEnumerable<MovieModel>);
            var expectedErrorMessage = "Please include tmdb_5000_movies.csv and tmdb_5000_credits.csv in wwwroot root folder.";

            try
            {
                // Act
                var actualResult = _controllerHelper.GetMovies();

                // Assert
                Assert.IsInstanceOfType(actualResult, expectedType);
                Assert.IsTrue(actualResult.Count() > 0);
            }
            catch (FileNotFoundException ex)
            {
                Assert.AreEqual(expectedErrorMessage, ex.Message);
            }
        }

        [TestMethod]
        public void Get_ReturnsListOfUserPreferences()
        {
            //Arrange 
            var expectedType = typeof(IEnumerable<UserPreferenceModel>);

            // Act
            var actualResult = _controllerHelper.GetUserPreferences();

            // Assert
            Assert.IsInstanceOfType(actualResult, expectedType);
            Assert.IsTrue(actualResult.Count() > 0);
        }

        [TestMethod]
        public void Get_ValidUserId_ReturnsUserPreference()
        {
            //Arrange
            var expectedType = typeof(UserPreferenceModel);
            var validUserId = 100;

            // Act
            var actualResult = _controllerHelper.GetUserPreferencesByUserId(validUserId);

            // Assert
            Assert.IsInstanceOfType(actualResult, expectedType);
            Assert.IsNotNull(actualResult);
        }

        [TestMethod]
        public void Get_InvalidUserId_ReturnsNull()
        {
            //Arrange
            var invalidUserId = 1;

            // Act
            var actualResult = _controllerHelper.GetUserPreferencesByUserId(invalidUserId);

            // Assert
            Assert.IsNull(actualResult);
        }
    }
}
