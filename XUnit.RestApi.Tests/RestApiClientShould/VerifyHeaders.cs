using System.Net;
using System.Threading.Tasks;
using Xunit;
using XUnit.RestApi.Tests.Helpers;

namespace XUnit.RestApi.Tests.RestApiClientShould
{
    public partial class RestApiClientShould
    {
        public class When_Verifying_Response_Headers : RestApiClientTestSuite
        {
            [Fact]
            public async Task Check_Location_Header()
            {
                const string expectedLocation = "http://test/location";
                
                var testId = GivenResponseLocation(expectedLocation);

                await
                    Api
                        .When()
                        .Get($"responseWithLocation/{testId}")
                        .Then()
                        .Status(HttpStatusCode.Created)
                        .Location(expectedLocation);
            }
            
        }
    }
}