using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;


namespace DolmToken.Models
{
    class cmcAPI
    {
        public static string API_KEY = System.IO.File.ReadAllText(@"C:\Users\dietz\Documents\.4BHWII\DolmToken\DolmTokenRepos-main\DolmTokenRepos\DolmTokenRepos\shortguido\DolmTokenRepos\DolmToken\DolmToken\api.txt");

        public static string makeAPICall()
        {
            var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest");

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["start"] = "1";
            queryString["limit"] = "50";
            queryString["convert"] = "EUR";

            URL.Query = queryString.ToString();

            var client = new WebClient();
            client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);
            client.Headers.Add("Accepts", "application/json");
            return client.DownloadString(URL.ToString());

        }

    }
}
