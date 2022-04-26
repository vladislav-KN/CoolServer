using CoolServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CoolServer.MessageTransfer
{
    public class TcpConnection : ISocetConectable<TcpClient>
    {
        private IDictionary<Task<TcpClient>, Tuple<TcpListener, User>> ipUsers;
        private int port;
        TcpConnection()
        {
            port = 30000;
            ipUsers = new Dictionary<Task<TcpClient>, Tuple<TcpListener, User>>();
        }
        TcpConnection(int p)
        {
            port = p;
            ipUsers = new Dictionary<Task<TcpClient>, Tuple<TcpListener, User>>();
        }

        int ISocetConectable<TcpClient>.Port { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Add(TcpClient client, User user)
        {
            throw new NotImplementedException();
        }

        public void BrakeConnection(TcpClient client)
        {
            throw new NotImplementedException();
        }

        public void CheckConnection(TcpClient client)
        {
            throw new NotImplementedException();
        }

        public void CheckConnections()
        {
            throw new NotImplementedException();
        }

        public TcpClient GetClient(User user)
        {
            throw new NotImplementedException();
        }

        public User GetUser(TcpClient client)
        {
            throw new NotImplementedException();
        }

        public bool SendData(IData data)
        {
            throw new NotImplementedException();
        }

         
    }
}