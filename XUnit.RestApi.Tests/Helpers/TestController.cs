using System;
using System.Web.Http;
using Newtonsoft.Json;

namespace XUnit.RestApi.Tests.Helpers
{
    public class TestController : ApiController
    {
        private readonly TestValuesRepository _testValuesRepository;

        public TestController(TestValuesRepository testValuesRepository)
        {
            _testValuesRepository = testValuesRepository;
        }
        
        [HttpGet]
        [Route("test200ok")]
        public IHttpActionResult Test200Ok()
        {
            return Ok();
        }

        [HttpGet]
        [Route("getContent/{id}")]
        public object TestContent(Guid id)
        {
            return _testValuesRepository.Get(id);
        }

        [HttpGet]
        [Route("responseWithLocation/{id}")]
        public IHttpActionResult TestLocation(Guid id)
        {
            return Created<object>(new Uri((string) _testValuesRepository.Get(id)), new { test = "test" });
        }

        [HttpGet]
        [Route("serverError/{id}")]
        public void TestException(Guid id)
        {
            throw new Exception((string)_testValuesRepository.Get(id));
        }

        [HttpPost]
        [Route("putNameToValues_POST/{id}")]
        public IHttpActionResult PutNameToValuesPost(Guid id, [FromBody] User user)
        {
            _testValuesRepository.Add(id, user.Name);
            return Ok();
        }
        
        [HttpPost]
        [Route("putNameToValues_PUT/{id}")]
        public IHttpActionResult PutNameToValuesPut(Guid id, [FromBody] User user)
        {
            _testValuesRepository.Add(id, user.Name);
            return Ok();
        }

        [HttpDelete]
        [Route("deleteValueById/{id}")]
        public IHttpActionResult DeleteValueById(Guid id)
        {
            _testValuesRepository.Remove(id);
            return Ok();
        }

    }

    public class User
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}