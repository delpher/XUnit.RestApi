using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace XUnit.RestApi
{
    public static class ContentReader
    {
        public static Task<JToken> Read(HttpResponseMessage response)
        {
            return response.Content
                .ReadAsStringAsync()
                .ContinueWith(ParseContent);
        }

        private static JToken ParseContent(Task<string> contentTask)
        {
            return JToken.Parse(contentTask.Result);
        }
    }
}