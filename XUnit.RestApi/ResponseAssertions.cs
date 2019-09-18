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
        private readonly List<Task> _tasks;

        public ResponseAssertions(Task<HttpResponseMessage> response)
        {
            _response = response;
            _tasks = new List<Task>();
        }

        public ResponseAssertions Status(HttpStatusCode expectedStatus)
        {
            AddTask(WhenResponse(response => Assert.Equal(expectedStatus, response.StatusCode)));
            return this;
        }

        public ResponseAssertions Location(string expectedLocation)
        {
            AddTask(WhenResponse(response => Assert.Equal(expectedLocation, "" + response.Headers.Location)));

            return this;
        }

        public ResponseAssertions Body<T>(Action<T> assertions)
        {
            AddTask(VerifyBody(content =>
                assertions(content.ToObject<T>())));
            return this;
        }

        public ResponseAssertions Body(Action<dynamic> assertions)
        {
            AddTask(VerifyBody(assertions));
            return this;
        }

        public ResponseAssertions Body(IObjectComparer comparer)
        {
            AddTask(VerifyBody(comparer.Validate));
            return this;
        }

        private Task VerifyBody(Action<JToken> assertions)
        {
            return WhenResponse(ContentReader.Read)
                .Unwrap()
                .ContinueWith(RunAssertions(assertions));
        }

        private static Action<Task<JToken>> RunAssertions(Action<JToken> assertions)
        {
            return bodyTask => assertions(bodyTask.Result);
        }

        private Task<T> WhenResponse<T>(Func<HttpResponseMessage, T> action)
        {
            return _response.ContinueWith(responseTask => action(responseTask.Result));
        }
        
        private Task WhenResponse(Action<HttpResponseMessage> action)
        {
            return _response.ContinueWith(responseTask => action(responseTask.Result));
        }


        private void AddTask(Task task)
        {
            _tasks.Add(task);
        }

        internal async Task Verify()
        {
            await Task.WhenAll(_tasks);
        }
    }
}