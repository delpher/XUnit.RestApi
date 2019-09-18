using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace XUnit.RestApi
{
    public static class ContentReader
    {
        public static Task<JToken> Read(Task<HttpResponseMessage> response)
        {
            return response.ContinueWith(ReadContentAsString)
                .Unwrap()
                .ContinueWith(ParseContent);
        }

        private static Task<string> ReadContentAsString(Task<HttpResponseMessage> responseTask)
        {
            return responseTask.Result.Content.ReadAsStringAsync();
        }

        private static JToken ParseContent(Task<string> contentTask)
        {
            return JToken.Parse(contentTask.Result);
        }
    }
}