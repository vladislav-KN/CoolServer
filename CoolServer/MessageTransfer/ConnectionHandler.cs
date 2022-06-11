using CoolServer.Controllers.CModels;
using CoolServer.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CoolServer.MessageTransfer
{
    public abstract class Connection : IConnection
    {

        internal string jsonString;
        internal int port;
        public int Port { get => port; }
        public virtual void SendData(Transfer data)
        {
            jsonString = JsonConvert.SerializeObject(data);
        }

        public async Task Handler(IDisposable result)
        {
             
        }
    }

    public class ConnectionHandler: Connection
    {
        private IDictionary<Task<TcpClient>, Tuple<TcpListener, User>> ipUsers;
        
        public ConnectionHandler()
        {
            port = 30000;
            ipUsers = new Dictionary<Task<TcpClient>, Tuple<TcpListener, User>>();
        }
        public ConnectionHandler(int p)
        {
            port = p;
            ipUsers = new Dictionary<Task<TcpClient>, Tuple<TcpListener, User>>();
        }

        public IDictionary<Task<TcpClient>, Tuple<TcpListener, User>> IpUsers { get { return ipUsers; } } 
        
        public async Task CheckConectedUsers()
        {

        }

        public async Task Handler(TcpClient client)
        {

        }

        public override void SendData(Transfer data)
        {
            base.SendData(data);
      
        }
 
    }
}