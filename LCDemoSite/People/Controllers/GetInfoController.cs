using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Servise.DataProviders;
using Servise.Dto;

namespace People.Controllers
{
    public class GetInfoController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var people = GetData();

            if (people == null)
                return new HttpResponseMessage(HttpStatusCode.ExpectationFailed);

            people.Qoute = GeekJokesApiDataProvider.GetData();

            var keyPeople = SavePeople(people);

            AddPoem(keyPeople);

            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        private PeopleDto GetData()
        {
            var provider = new RandomUserDataProvider();
            var jsonData = provider.GetData().Result;

            if (jsonData == null || !jsonData.Results.Any())
                return null;

            var people = jsonData.Results.First();

            return new PeopleDto
            {
                FirstName = people.Name.First,
                Gender = people.Gender,
            };
        }

        private string SavePeople(PeopleDto people)
        {
            string result = null;
            using (var dbManager = new DataBaseManager())
            {
                result = dbManager.SetPeople(people);
            }

            return result;
        }

        private void AddPoem(string key)
        {
            using (var client = new HttpClient())
            {
                var result = client.GetAsync("http://localhost:50252/api/GetPoem/" +key).Result;
                if (result.IsSuccessStatusCode)
                {

                }
            }
        }
    }
}
