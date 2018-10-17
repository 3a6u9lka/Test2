using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Servise.DataProviders;
using Servise.Dto;
using Servise.FuzzyString;

namespace Poem.Controllers
{
    public class GetPoemController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpResponseMessage(HttpStatusCode.OK);

            var poems = GetData(id);

            SavePoem(poems);

            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        private IEnumerable<PoemDto> GetData(string key)
        {
            var provider = new RandomPoemDataProvider();
            var jsonData = provider.GetData().Result;

            foreach (var jsonPoemDto in jsonData)
            {
                if (string.IsNullOrEmpty(jsonPoemDto?.Content))
                    continue;

                var dics = 0d;
                var lastStrin = jsonPoemDto.Content
                    .Split('.', '?', '!')
                    .Aggregate((cur, next) =>
                    {
                        dics += cur.JaroWinklerDistance(next);
                        cur = next;
                        return cur;
                    });

                yield return new PoemDto
                {
                    UserKey = key,
                    Content = jsonPoemDto.Content,
                    Title = jsonPoemDto.Title,
                    Distance = dics,
                };
            }
        }

        private void SavePoem(IEnumerable<PoemDto> poems)
        {
            using (var dbManager = new DataBaseManager())
            {
                foreach (var poemDto in poems)
                {
                    dbManager.SetPeom(poemDto);
                }
            }
        }
    }
}
