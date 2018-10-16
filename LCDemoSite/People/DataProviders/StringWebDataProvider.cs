using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace People.DataProviders
{
    public class StringWebDataProvider
    {
        private static readonly string _url = "https://randomuser.me/api/";

        public static string GetData()
        {
            string data = null;
            using (var client = new HttpClient())
            {
                data = client.GetStringAsync(_url).Result;
            }
            return data;
        }
    }
}