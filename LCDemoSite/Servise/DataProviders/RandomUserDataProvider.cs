using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servise.Dto;

namespace Servise.DataProviders
{
    public class RandomUserDataProvider : JsonWebApiProvider<JsonPeopleDto>
    {
        public RandomUserDataProvider() : base("https://randomuser.me/api/")
        {
        }
    }
}
