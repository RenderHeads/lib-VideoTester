using System;
namespace LibVideoTester.Serialization
{
    public interface IDeserializer<T>
    {
        public T Deserialize(string contents);
    }
}

