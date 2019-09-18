using System.Web.Http;
using Owin;
using Unity;
using Unity.AspNet.WebApi;

namespace XUnit.RestApi.Tests.Helpers
{
    public static class TestConfig
    {
        public static void Configure(IAppBuilder app, IUnityContainer container)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.DependencyResolver = new UnityDependencyResolver(container);
            app.UseWebApi(config);
        }
    }
}