using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Jeopardy_Backend_IntegrationTests
{
    public class BaseIntegrationTests
    {
        protected async Task<IEnumerable<T>> GetItems<T>(string path, HttpClient client)
        {
            var response = await client.GetAsync(path);
            return JsonConvert.DeserializeObject<IEnumerable<T>>(await response.Content.ReadAsStringAsync());
        }

        protected async Task<int> GetItemsCount<T>(string path, HttpClient client)
        {
            return (await this.GetItems<T>(path, client)).Count();
        }

        protected StringContent CreateContent<T>(T body)
        {
            var serializedBody = JsonConvert.SerializeObject(body);
            return new StringContent(serializedBody, Encoding.UTF8, "application/JSON");
        }
    }
}
