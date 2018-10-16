using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using People.DataProviders;

namespace People.Controllers
{
    public class PeopleController : ApiController
    {
        public string GetInfo()
        {
            var peopleProvider = new JsonApiDataProvider();
            var people = peopleProvider.GetData();

            if (people == null)
                return "";

            people.Qoute = StringWebDataProvider.GetData();

            //SavePeople

            return "{ \"name\":\"John\" }";

        }
    }
}
