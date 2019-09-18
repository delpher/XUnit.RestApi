using Newtonsoft.Json.Linq;

namespace XUnit.RestApi
{
    public interface IObjectComparer
    {
        void Validate(JToken actual);
    }
}