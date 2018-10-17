using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Servise.DataProviders
{
    public class JsonWebApiProvider<T>
    {
        protected readonly string Url;

        public JsonWebApiProvider(string url)
        {
            Url = url;
        }

        public async Task<T> GetData()
        {
            T data = default(T);

            using (var client = new HttpClient())
            {
                var response = client.GetAsync(Url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var reader = await response.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<T>(reader.Normalize());
                }
            }

            return data;
        }

    }
}
