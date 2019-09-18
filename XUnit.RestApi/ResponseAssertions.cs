using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xunit;

namespace XUnit.RestApi
{
    public class ResponseAssertions
    {
        private readonly Task<HttpResponseMessage> _response;
        private readonly List<Task> _assertions;

        public ResponseAssertions(Task<HttpResponseMessage> response)
        {
            _response = response;
            _assertions = new List<Task>();
        }

        public ResponseAssertions Status(HttpStatusCode expectedStatus)
        {
            var checkTask = _response.ContinueWith(t => AssertStatusCode(expectedStatus, t.Result));
            _assertions.Add(checkTask);
            return this;
        }

        private static void AssertStatusCode(HttpStatusCode expectedStatus, HttpResponseMessage responseMessage)
        {
            Assert.Equal(expectedStatus, responseMessage.StatusCode);
        }

        public ResponseAssertions Body<T>(Action<T> bodyAssertions)
        {
            var bodyVerifyTask = _response
                .ContinueWith(responseTask => responseTask.Result.Content.ReadAsStringAsync())
                .Unwrap()
                .ContinueWith(contentTask => bodyAssertions(JToken.Parse(contentTask.Result).ToObject<T>()));

            _assertions.Add(bodyVerifyTask);

            return this;
        }

        public ResponseAssertions Body(Action<dynamic> bodyAssertions)
        {
            var bodyVerifyTask = _response
                .ContinueWith(responseTask => responseTask.Result.Content.ReadAsStringAsync())
                .Unwrap()
                .ContinueWith(contentTask => bodyAssertions(JToken.Parse(contentTask.Result)));

            _assertions.Add(bodyVerifyTask);

            return this;
        }

        public ResponseAssertions Body(IObjectComparer comparer)
        {
            var bodyVerifyTask = _response
                .ContinueWith(responseTask => responseTask.Result.Content.ReadAsStringAsync())
                .Unwrap()
                .ContinueWith(contentTask => comparer.Validate(JToken.Parse(contentTask.Result)));

            _assertions.Add(bodyVerifyTask);

            return this;
        }

        public ResponseAssertions Location(string expectedLocation)
        {
            var verifyLocationTask = _response
                .ContinueWith(responseTask => AssertLocation(expectedLocation, responseTask.Result));
            
            _assertions.Add(verifyLocationTask);
            
            return this;
        }

        private static void AssertLocation(string expectedLocation, HttpResponseMessage response)
        {
            Assert.Equal(expectedLocation, "" + response.Headers.Location);
        }

        internal async Task Verify()
        {
            await Task.WhenAll(_assertions);
        }
    }
}