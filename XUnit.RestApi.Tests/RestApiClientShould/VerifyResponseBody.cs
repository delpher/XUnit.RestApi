using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentAssertions.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            public async Task Fail_When_Properties_Mismatch()
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

            [Fact]
            public async Task Verify_Body_With_Comparer()
            {
                var original = new {name = "Bob", age = 32, position = "Manager"};
                var sample = new { name = "Bob", age = 32 };
                
                var id = GivenResponseContent(original);
                
                await
                    Api
                        .When()
                        .Get($"getContent/{id}")
                        .Then()
                        .Body(new Contains(sample));
            }

            [Fact]
            public async Task Fail_When_Comparer_Fails()
            {
                var bob = new {name = "Bob", age = 32, position = "Manager"};
                var mark = new { name = "Mark", age = 44 };
                
                var id = GivenResponseContent(bob);

                async Task FailingComparision() =>
                    await Api
                        .When()
                        .Get($"getContent/{id}")
                        .Then()
                        .Body(new Contains(mark));

                await Assert.ThrowsAnyAsync<XunitException>(FailingComparision);
            }

            public class User
            {
                [JsonProperty(PropertyName = "name")] public string Name { get; set; }

                [JsonProperty(PropertyName = "age")] public int Age { get; set; }
            }
        }
    }

    public class Contains : IObjectComparer
    {
        private readonly object _sample;

        public Contains(object sample)
        {
            _sample = sample;
        }

        public void Validate(JToken actual)
        {
            actual.Should().ContainSubtree(JToken.FromObject(_sample));
        }
    }
}