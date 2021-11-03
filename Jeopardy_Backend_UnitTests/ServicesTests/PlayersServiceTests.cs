using Jeopardy_Backend.Models;
using Jeopardy_Backend.Services;
using Jeopardy_Backend_TestCommon.DataSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Jeopardy_Backend_UnitTests.ServicesTests
{
    public class PlayersServiceTests : IDisposable
    {
        private readonly DatabaseFixture fixture;
        private readonly PlayersService service;

        public PlayersServiceTests()
        {
            this.fixture = new DatabaseFixture();
            this.service = new PlayersService(fixture.context);
        }

        public void Dispose()
        {
            this.fixture.Dispose();
        }

        [Fact]
        public async Task GetReturns_IEnumerableOfPlayers()
        {
            // Arrange
            var expectedPlayersCount = this.fixture.context.Players.Count();

            // Act
            var players = await this.service.GetPlayers();

            Assert.IsAssignableFrom<IEnumerable<Player>>(players);
            Assert.Equal(expectedPlayersCount, players.Count());
        }

        [Fact]
        public async Task UpdateRates_UpdatesRatesForPlayers()
        {
            // Arrange
            var initialPlayersRates = await GetPlayersRates();

            // Act
            await this.service.UpdateRates();
            var actualPlayersRates = await GetPlayersRates();

            Assert.NotEqual(initialPlayersRates, actualPlayersRates);
        }

        private async Task<IEnumerable<double>> GetPlayersRates()
        {
            var players = await this.service.GetPlayers();

            return players.Select(x => x.Rate);
        }
    }
}
