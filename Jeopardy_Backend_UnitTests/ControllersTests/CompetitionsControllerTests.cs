using Jeopardy_Backend.Controllers;
using Jeopardy_Backend.Models;
using Jeopardy_Backend.Services;
using Jeopardy_Backend_TestCommon.DataSetup;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Jeopardy_Backend_UnitTests.ControllersTests
{
    public class CompetitionsControllerTests : IDisposable
    {
        private readonly DatabaseFixture fixture;
        private readonly Mock<CompetitionsService> service;
        private readonly CompetitionsController controller;

        public CompetitionsControllerTests()
        {
            this.fixture = new DatabaseFixture();
            this.service = new Mock<CompetitionsService>(fixture.context);
            this.controller = new CompetitionsController(service.Object);
        }

        public void Dispose()
        {
            this.fixture.Dispose();
        }

        [Fact]
        public async Task Get_Returns_OkObjectResultWithIEnumerableOfCompetitions()
        {
            // Act
            var response = await this.controller.Get();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(response.Result) as OkObjectResult;

            Assert.IsAssignableFrom<IEnumerable<Competition>>(okObjectResult.Value);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task Post_Adds_Competition()
        {
            // Arrange
            var competition = new Competition() { Name = "ForthCompetition", Date = new DateTime(2021, 11, 14) };
            var initialCompetitionCount = await this.GetCompetitionsCount();

            // Act
            var response = await this.controller.Post(competition);

            var actualCompetitionResult = await this.GetCompetitionsCount();

            // Assert
            Assert.IsType<OkObjectResult>(response.Result);

            Assert.Equal(initialCompetitionCount + 1, actualCompetitionResult);
        }

        [Fact]
        public async Task Post_Returns_CompetitionAdded()
        {
            // Arrange
            var competition = new Competition() { Name = "FifthCompetition", Date = new DateTime(2021, 12, 14) };

            // Act
            var response = await this.controller.Post(competition);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(response.Result) as OkObjectResult;
            var actualCompetition = (Competition)okObjectResult.Value;

            Assert.Equal(competition, actualCompetition);
        }

        private async Task<int> GetCompetitionsCount()
        {
            var getCompetitionsResponse = await this.controller.Get();
            var competitions = getCompetitionsResponse.Result as OkObjectResult;
            return ((IEnumerable<Competition>)competitions.Value).Count();
        }
    }
}
