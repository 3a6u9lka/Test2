using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace People.DataProviders
{
    public class GeekJokesApiDataProvider
    {
        private static readonly string _url = "https://geek-jokes.sameerkumar.website/api";

        public static string GetQuote()
        {
            using (WebClient webClient = new WebClient())
            {
                return webClient.DownloadString(_url);
            }
        }
    }
}