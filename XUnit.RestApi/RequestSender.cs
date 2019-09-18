using System.Net.Http;
using System.Net.Http.Formatting;

namespace XUnit.RestApi
{
    public class RequestSender
    {
        private readonly HttpClient _client;

        public RequestSender(HttpClient client)
        {
            _client = client;
        }

        public ResponseValidator Get(string url)
        {
            return new ResponseValidator(_client.GetAsync(url));
        }

        public ResponseValidator Post(string url, object postData)
        {
            return new ResponseValidator(_client.PostAsync(url, MakeObjectContent(postData)));
        }

        private static ObjectContent<object> MakeObjectContent(object postData)
        {
            return new ObjectContent<object>(postData, new JsonMediaTypeFormatter());
        }

        public ResponseValidator Delete(string url)
        {
            return new ResponseValidator(_client.DeleteAsync(url));
        }
    }
}