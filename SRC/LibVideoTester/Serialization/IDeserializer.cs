using System;
namespace LibVideoTester.Serialization {
  public interface IDeserializer<T> {
    public bool TryDeserialize(string contents, out T objOut);
  }
}
