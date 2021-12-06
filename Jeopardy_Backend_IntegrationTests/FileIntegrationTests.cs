using Jeopardy_Backend.Constants;
using Jeopardy_Backend_TestCommon;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Jeopardy_Backend_IntegrationTests.FileIntegrationTests
{
    [CollectionDefinition("S3 collection")]
    public class ApiCollection : ICollectionFixture<AppFixture>
    {
    }

    [Collection("S3 collection")]
    public class FileIntegrationTests : IAsyncLifetime
    {
        private readonly HttpClient client;
        private readonly AppFixture fixture;

        public FileIntegrationTests(AppFixture fixture)
        {
            this.client = fixture.Client;
            this.fixture = fixture;
        }

        [Fact]
        public async Task GetFile_Returns_OkStatusCode()
        {
            // Act
            var response = await this.client.GetAsync(ApiConstants.GetFileRout + this.fixture.Configuration.GetSection("TestFleName").Value);
            var filecontent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(this.fixture.Configuration.GetSection("TestFleContent").Value, filecontent);
        }

        public async Task InitializeAsync()
        {

            await this.fixture.CreateTestBucket();
            await this.fixture.UploadTestFile();
        }

        public async Task DisposeAsync()
        {
        }
    }
}
