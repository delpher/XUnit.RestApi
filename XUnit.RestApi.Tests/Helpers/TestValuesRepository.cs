using System;
using System.Collections.Concurrent;

namespace XUnit.RestApi.Tests.Helpers
{
    public class TestValuesRepository
    {
        private readonly ConcurrentDictionary<Guid, object> _content;

        public TestValuesRepository()
        {
            _content = new ConcurrentDictionary<Guid, object>();
        }

        public void Add(Guid id, object content)
        {
            _content[id] = content;
        }

        public object Get(Guid id)
        {
            return _content[id];
        }

        public void Remove(Guid id)
        {
            _content.TryRemove(id, out var value);
        }

        public bool HasValue(Guid id)
        {
            return _content.ContainsKey(id);
        }
    }
}