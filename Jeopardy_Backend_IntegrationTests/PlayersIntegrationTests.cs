using Jeopardy_Backend.Constants;
using Jeopardy_Backend.Models;
using Jeopardy_Backend_TestCommon;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Jeopardy_Backend_IntegrationTests.PlayersIntegrationTests
{
    [CollectionDefinition("Players collection")]
    public class ApiCollection : ICollectionFixture<AppFixture>
    {
    }

    [Collection("Players collection")]
    public class PlayersIntegrationTests : BaseIntegrationTests
    {
        private readonly HttpClient client;
        private readonly AppFixture fixture;

        public PlayersIntegrationTests(AppFixture fixture)
        {
            this.client = fixture.Client;
            this.fixture = fixture;
        }

        [Fact]
        public async Task GetPlayers_Returns_OkStatusCode()
        {
            // Act
            var response = await this.client.GetAsync(ApiConstants.PlayersRout);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostPlayers_Returns_UnauthorizedForUnauthorizedUser()
        {
            // Arrange
            var body = new Player() { Name = "Bett" };
            var serializedBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(serializedBody);

            // Act
            var response = await this.client.PostAsync(ApiConstants.PlayersRout, content);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task PostPlayers_Adds_Player()
        {
            // Arrange
            var authorizedClient = await this.fixture.CreateAuthorizedClient();

            var initialPlayersCount = await this.GetItemsCount<Player>(ApiConstants.PlayersRout, this.client);

            var body = new Player() { Name = "Bett" };
            var serializedBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(serializedBody, Encoding.UTF8, "application/JSON");

            // Act
            var response = await authorizedClient.PostAsync(ApiConstants.PlayersRout, content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var actualPlayersCount = await this.GetItemsCount<Player>(ApiConstants.PlayersRout, this.client);

            Assert.Equal(initialPlayersCount + 1, actualPlayersCount);
        }

        [Fact]
        public async Task PostPlayer_ReturnsPlayerCreated()
        {
            // Arrange
            var authorizedClient = await this.fixture.CreateAuthorizedClient();

            var name = "Leo";
            var body = new Player() { Name = name };
            var content = this.CreateContent(body);

            // Act
            var response = await authorizedClient.PostAsync(ApiConstants.PlayersRout, content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.NotNull((await this.GetItems<Player>(ApiConstants.PlayersRout, this.client)).FirstOrDefault(x => x.Name == name));
        }

        [Fact]
        public async Task UpdatePlayer_UpdatesPlayerCreated()
        {
            // Arrange
            var authorizedClient = await this.fixture.CreateAuthorizedClient();

            var name = "John";
            var updatedName = "Jane";
            await this.AddPlayer(name, authorizedClient);
            var player = (await this.GetItems<Player>(ApiConstants.PlayersRout, this.client)).FirstOrDefault(x => x.Name == name);
            player.Name = updatedName;

            var content = this.CreateContent(player);

            // Act
            var response = await authorizedClient.PutAsync(ApiConstants.PlayersRout + "/" + player.Id, content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var updatedPlayer = (await this.GetItems<Player>(ApiConstants.PlayersRout, this.client)).FirstOrDefault(x => x.Name == updatedName);

            Assert.NotNull(updatedPlayer);
            Assert.Equal(player.Id, updatedPlayer.Id);
        }

        [Fact]
        public async Task UpdatePlayer_ReturnsPlayerUpdated()
        {
            // Arrange
            var authorizedClient = await this.fixture.CreateAuthorizedClient();

            var name = "Marta";
            var updatedName = "Greg";
            await this.AddPlayer(name, authorizedClient);

            var player = (await this.GetItems<Player>(ApiConstants.PlayersRout, this.client)).FirstOrDefault(x => x.Name == name);
            player.Name = updatedName;

            var content = this.CreateContent(player);

            // Act
            var response = await authorizedClient.PutAsync(ApiConstants.PlayersRout + "/" + player.Id, content);

            // Assert
            var responsePlayer = Assert.IsType<Player>(JsonConvert.DeserializeObject<Player>(await response.Content.ReadAsStringAsync())) as Player;
            Assert.Equal(updatedName, responsePlayer.Name);
        }

        private async Task AddPlayer(string name, HttpClient client)
        {
            var body = new Player() { Name = name };
            var content = this.CreateContent(body);
            await client.PostAsync(ApiConstants.PlayersRout, content);
        }
    }
}
