using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servise.Dto;

namespace Servise.DataProviders
{
    public class RandomPoemDataProvider : JsonWebApiProvider<JsonPoemDto>
    {
        public RandomPoemDataProvider() : base("https://www.poemist.com/api/v1/randompoems")
        {
        }
    }
}
