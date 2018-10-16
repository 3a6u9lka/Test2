using System.Net;
using System.Net.Http;
using System.Web.Http;
using Servise.DataProviders;
using Servise.Dto;

namespace Poem.Controllers
{
    public class GetPoemController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                return new HttpResponseMessage(HttpStatusCode.OK);

            var poem = GetData(key);

            if (poem == null)
                return new HttpResponseMessage(HttpStatusCode.ExpectationFailed);

            SavePoem(poem);

            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        private PoemDto GetData(string key)
        {
                var provider = new RandomPoemDataProvider();
                var jsonData = provider.GetData().Result;

            if (string.IsNullOrEmpty(jsonData?.Content))
                return null;

            return new PoemDto
            {
                UserKey = key,
                Content = jsonData.Content,
                Title = jsonData.Title,
            };
        }

        private void SavePoem(PoemDto poem)
        {
            //todo : Save with DAtaBase
        }
    }
}
