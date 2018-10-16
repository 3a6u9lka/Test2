using System.Net;

namespace Servise.DataProviders
{
    public class GeekJokesApiDataProvider
    {
        private static readonly string _url = "https://geek-jokes.sameerkumar.website/api";

        public static string GetData()
        {
            using (WebClient webClient = new WebClient())
            {
                return webClient.DownloadString(_url);
            }
        }
    }
}