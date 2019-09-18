using System.Net;
using System.Threading.Tasks;
using Xunit;
using XUnit.RestApi.Tests.Helpers;
using Xunit.Sdk;

namespace XUnit.RestApi.Tests.RestApiClientShould
{
    public partial class RestApiClientShould
    {
        public class When_Verifying_Response_Status : RestApiClientTestSuite
        {
            [Fact]
            public async Task Succeed_On_Status_Match()
            {
                await Api
                    .When()
                    .Get("test200ok")
                    .Then()
                    .Status(HttpStatusCode.OK);
            }

            [Fact]
            public async Task Throw_On_Status_Mismatch()
            {
                async Task VerifyWrongStatus() =>
                    await Api.When()
                        .Get("test200ok")
                        .Then()
                        .Status(HttpStatusCode.Accepted);

                await Assert.ThrowsAnyAsync<XunitException>(VerifyWrongStatus);
            }

            [Fact]
            public async Task Provide_Response_Content_On_Internal_Server_Error()
            {
                const string expectedMessage = "test-exception-message";

                var id = GivenExceptionMessage(expectedMessage);

                await Api.When()
                    .Get($"serverError/{id}")
                    .Then()
                    .Status(HttpStatusCode.InternalServerError)
                    .Body(exception => Assert.Contains(expectedMessage, (string) exception["ExceptionMessage"]));
            }
        }
    }
}