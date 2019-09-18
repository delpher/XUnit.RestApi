using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using XUnit.RestApi.Tests.Helpers;

namespace XUnit.RestApi.Tests.RestApiClientShould
{
    public partial class RestApiClientShould
    {
        public class When_Using_Different_Request_Types : RestApiClientTestSuite
        {
            [Fact]
            public async Task Pass_Data_With_Post_Request()
            {
                const string expectedUserName = "John";
                var id = Guid.NewGuid();
                var user = new { name = expectedUserName };
                
                await Api
                    .When()
                    .Post($"putNameToValues_POST/{id}", user)
                    .Then()
                    .Status(HttpStatusCode.OK);
                
                Assert.Equal(expectedUserName, ValuesRepository.Get(id));
            }
            
            [Fact]
            public async Task Pass_Data_With_Put_Request()
            {
                const string expectedName = "John Smith";
                var id = Guid.NewGuid();
                var user = new { name = expectedName };
                
                await Api
                    .When()
                    .Post($"putNameToValues_PUT/{id}", user)
                    .Then()
                    .Status(HttpStatusCode.OK);
                
                Assert.Equal(expectedName, ValuesRepository.Get(id));
            }
            
            [Fact]
            public async Task Verify_Response_From_Delete_Request()
            {
                var id = AddTestValue("test-value");
                
                Assert.True(ValuesRepository.HasValue(id));
                
                await
                    Api
                        .When()
                        .Delete($"deleteValueById/{id}")
                        .Then()
                        .Status(HttpStatusCode.OK);
                
                Assert.False(ValuesRepository.HasValue(id));
            }
        }
    }
}