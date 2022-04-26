using CoolServer.MessageTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CoolServer.Interfaces
{
    public interface ISocetConectable<T>
        where T: IDisposable
    {
        public int Port { get; internal set; }
        public User GetUser(T client);
        public T GetClient(User user);
        public bool Add(T client, User user);
        public void BrakeConnection(T client);
        public void CheckConnection(T client);
        public void CheckConnections();
        public bool SendData(IData data);
    }
}
