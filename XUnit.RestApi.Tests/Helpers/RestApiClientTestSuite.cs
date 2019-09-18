using System;
using Microsoft.Owin.Testing;
using Unity;

namespace XUnit.RestApi.Tests.Helpers
{
    public class RestApiClientTestSuite : IDisposable
    {
        private readonly TestServer _server;

        protected readonly TestValuesRepository ValuesRepository;
        protected readonly RestApiClient Api;

        protected RestApiClientTestSuite()
        {
            var container = new UnityContainer();
            container.RegisterInstance(ValuesRepository = new TestValuesRepository());
            _server = TestServer.Create(appBuilder => TestConfig.Configure(appBuilder, container));
            Api = new RestApiClient(_server.Handler);
        }

        protected Guid GivenResponseContent(object content)
        {
            return AddTestValue(content);
        }

        protected Guid GivenResponseLocation(string location)
        {
            return AddTestValue(location);
        }

        protected Guid GivenExceptionMessage(string message)
        {
            return AddTestValue(message);
        }

        public Guid AddTestValue(object content)
        {
            var id = Guid.NewGuid();
            ValuesRepository.Add(id, content);
            return id;
        }

        public void Dispose()
        {
            _server?.Dispose();
        }
    }
}