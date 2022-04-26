using System;
namespace CoolServer.Interfaces
{
    public interface IData
    {
        public byte[] Buffer { get; set; }
        public void ToByte(object data);
        public object ToObject();
    }
}