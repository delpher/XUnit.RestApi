using System;
using System.Net.Http;

namespace XUnit.RestApi
{
    public class RestApiClient

    {
        private readonly HttpClient _client;

        public RestApiClient(HttpMessageHandler handler)
        {
            _client = new HttpClient(handler) { BaseAddress = new Uri("http://test") };
        }

        public RequestSender When()
        {
            return new RequestSender(_client);
        }
    }
}