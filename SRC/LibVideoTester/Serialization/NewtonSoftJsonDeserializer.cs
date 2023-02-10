using System;
using Newtonsoft.Json;
namespace LibVideoTester.Serialization
{
    public class NewtonSoftJsonDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(string contents)
        {
            return JsonConvert.DeserializeObject<T>(contents);
        }
    }
}

