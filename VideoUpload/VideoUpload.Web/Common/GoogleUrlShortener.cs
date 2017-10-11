using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace VideoUpload.Web.Common
{
    public class GoogleUrlShortener
    {
        private const string googleUrlShorternerUri = "https://www.googleapis.com/urlshortener/v1/url";
        private const string apiKey = "AIzaSyDxIMQC1IRj5KnZv7FSHKhsM1qhCmJUGq0";

        public static async Task<HttpResponseMessage> GetUrlShortenerAsync(string url)
        {
            //longUrl is case sensitive.
            var urlEntry = new { longUrl = url };

            //add content to be passed to the request.
            var content = new StringContent(JsonConvert.SerializeObject(urlEntry));
            
            //this is required in the request content else will return unsupported content error
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{ googleUrlShorternerUri }?key={ apiKey }");
            request.Content = content;

            var client = new HttpClient();
            var response = await client.SendAsync(request);

            return response;
        }
    }

    //For deserialization
    public class ShortenedUrlInfo
    {
        //shortened url of the given long url
        public string ID { get; set; }
        public string Kind { get; set; }
        //long url you have submitted
        public string LongUrl { get; set; }
    }
}