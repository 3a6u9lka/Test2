using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using People.Models;

namespace People.DataProviders
{
    public class JsonApiDataProvider
    {
        private readonly string _url = "https://randomuser.me/api/";

        private async Task<JsonPeopleDto> GetAsyncPeople()
        {
            JsonPeopleDto people = null;

            using (HttpClient client = new HttpClient())
            {
                var reader = await client.GetStringAsync(_url);
                people = JsonConvert.DeserializeObject<JsonPeopleDto>(reader);
            }

            return people;
        }

        public PeopleDto GetData()
        {
            var jsonData = GetAsyncPeople().Result;

            if (jsonData == null || !jsonData.Results.Any())
                return null;

            var people = jsonData.Results.First();

            return new PeopleDto
            {
                FirstName = people.Name.First,
                Gender = people.Gender,
            };
        }

    }
}