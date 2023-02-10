using System;
using Newtonsoft.Json;
namespace LibVideoTester
{
    public class StandardJsonDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(string contents)
        {
            return JsonConvert.DeserializeObject<T>(contents);
        }
    }
}

