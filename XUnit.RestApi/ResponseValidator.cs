using System.Net.Http;
using System.Threading.Tasks;

namespace XUnit.RestApi
{
    public class ResponseValidator
    {
        private readonly Task<HttpResponseMessage> _response;

        public ResponseValidator(Task<HttpResponseMessage> response)
        {
            _response = response;
        }

        public ResponseAssertions Then()
        {
            return new ResponseAssertions(_response);
        }
    }
}