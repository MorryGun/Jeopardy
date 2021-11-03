using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Jeopardy_Backend.Constants;
using Jeopardy_Backend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Jeopardy_Backend_TestCommon
{
    public class AppFixture : IDisposable
    {
        private readonly TestServer server;

        public HttpClient Client { get; }

        public IConfigurationRoot Configuration { get; }

        public AppFixture()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseEnvironment("Test")
                .UseStartup<TestStartUp>();

            this.Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            this.server = new TestServer(builder);

            this.Client = server.CreateClient();
            this.Client.BaseAddress = new Uri(Configuration.GetSection("TestBaseAddress").Value);
        }

        public void Dispose()
        {
            this.Client.Dispose();
            this.server.Dispose();
        }

        public async Task<HttpClient> CreateAuthorizedClient()
        {
            var client = server.CreateClient();
            client.BaseAddress = new Uri(Configuration.GetSection("TestBaseAddress").Value);

            var userData = new Credentials() { Email = Configuration.GetSection("TestUsername").Value, Password = Configuration.GetSection("TestPassword").Value };
            var serializedBody = JsonConvert.SerializeObject(userData);
            var content = new StringContent(serializedBody, Encoding.UTF8, "application/JSON");

            var response = await client.PostAsync(ApiConstants.RegisterRout, content);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                response = await client.PostAsync(ApiConstants.LoginRout, content);

            var token = JsonExtensions.DeserializeFromJson<string>(await response.Content.ReadAsStringAsync());
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }
    }
}
