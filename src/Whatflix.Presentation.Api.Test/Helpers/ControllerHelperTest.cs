using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
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
            // Act
            var actualResult = _controllerHelper.GetMovies();

            // Assert
            Assert.IsInstanceOfType(actualResult, typeof(IEnumerable<MovieModel>));
            Assert.IsTrue(actualResult.Count() > 0);
        }

        [TestMethod]
        public void Get_ReturnsListOfUserPreferences() 
        {
            // Act
            var actualResult = _controllerHelper.GetUserPreferences();

            // Assert
            Assert.IsInstanceOfType(actualResult, typeof(IEnumerable<UserPreferenceModel>));
            Assert.IsTrue(actualResult.Count() > 0);
        }

        [TestMethod]
        public void Get_ValidUserId_ReturnsUserPreference() 
        {
            //Arrange
            int validUserId = 100;

            // Act
            var actualResult = _controllerHelper.GetUserPreferencesByUserId(validUserId);

            // Assert
            Assert.IsInstanceOfType(actualResult, typeof(UserPreferenceModel));
            Assert.IsNotNull(actualResult);
        }

        [TestMethod]
        public void Get_InvalidUserId_ReturnsNull()
        {
            //Arrange
            int invalidUserId = 1;

            // Act
            var actualResult = _controllerHelper.GetUserPreferencesByUserId(invalidUserId);

            // Assert
            Assert.IsNull(actualResult);
        }

    }
}
