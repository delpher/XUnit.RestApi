using System.Runtime.CompilerServices;

namespace XUnit.RestApi
{
    public static class ResponseAssertionsTaskExtensions
    {
        public static TaskAwaiter GetAwaiter(this ResponseAssertions validator)
        {
            return validator.Verify().GetAwaiter();
        }
    }
}