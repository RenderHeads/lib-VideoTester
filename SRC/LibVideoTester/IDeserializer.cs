using System;
namespace LibVideoTester
{
    public interface IDeserializer<T>
    {
        public T Deserialize(string contents);
    }
}

