using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using XUnit.RestApi.Tests.Helpers;
using Xunit.Sdk;

namespace XUnit.RestApi.Tests.RestApiClientShould
{
    public partial class RestApiClientShould
    {
        public class Wen_Verifying_Response_Body : RestApiClientTestSuite
        {
            [Fact]
            public async Task Verify_Body_Properties()
            {
                var id = GivenResponseContent(new {name = "John"});
                await Api
                    .When()
                    .Get($"getContent/{id}")
                    .Then()
                    .Body(user => Assert.Equal("John", (string) user.name));
            }

            [Fact]
            public async Task Fail_When_Assertions_Fail()
            {
                var id = GivenResponseContent(new {name = "Smith"});

                async Task VerifyPropertyFailure() =>
                    await Api
                        .When()
                        .Get($"getContent/{id}")
                        .Then()
                        .Body(user => Assert.Equal("John", (string) user.name));

                await Assert.ThrowsAnyAsync<XunitException>(VerifyPropertyFailure);
            }

            [Fact]
            public async Task Provide_Strongly_Typed_Body()
            {
                var id = GivenResponseContent(new {name = "Mary", age = 32});
                await Api
                    .When()
                    .Get($"getContent/{id}")
                    .Then()
                    .Body<User>(user =>
                    {
                        Assert.Equal("Mary", user.Name);
                        Assert.Equal(32, user.Age);
                    });
            }

            public class User
            {
                [JsonProperty(PropertyName = "name")] public string Name { get; set; }

                [JsonProperty(PropertyName = "age")] public int Age { get; set; }
            }
        }
    }
}