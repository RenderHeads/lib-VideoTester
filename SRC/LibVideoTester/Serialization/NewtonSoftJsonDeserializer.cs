using System;
using Newtonsoft.Json;
namespace LibVideoTester.Serialization
{
    
    public class NewtonSoftJsonDeserializer<T> : IDeserializer<T>
    {
        public bool TryDeserialize(string contents, out T result)
        {
            
            try
            {
                result = JsonConvert.DeserializeObject<T>(contents);
                return true;
            }
            catch
            {
             // just supress the error for now  as we assume JSON is not valid?  We should bubble up the error in the future
            }
            result = default(T);
            return false;
        }
    }
}

