using CoolServer.Controllers.CModels;
using CoolServer.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
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
            var stream = client.GetStream();
            var binf = new BinaryFormatter();
            var message = binf.Deserialize(stream) as TransferMessages;
            switch (message.action)
            {
                case ACTION.CHNG:
                    //Заносим изменения в бд
                    CoolApiModels.Messages.MessageNewDetails messageNew = new CoolApiModels.Messages.MessageNewDetails()
                    {
                        Text = message.Message.Text,
                        Attachments = message.Message.Attachments,
                        IsViewed = message.Message.IsViewed
                    };
                    _ = RequestApi<CoolApiModels.Messages.MessageDetails, CoolApiModels.Messages.MessageNewDetails>.Put(messageNew, $"/api/Messages/{message.Message.Id}", message.Token);
                    break;
                case ACTION.DEL:
                    //Заносим изменения в бд
                    _ = RequestApi<object,object>.Delete($"/api/Messages/{message.Message.Id}", message.Token);
                    break;
                case ACTION.SEND:
                    CoolApiModels.Messages.NewMessageDetails newMessage = new CoolApiModels.Messages.NewMessageDetails()
                    {
                        ChatId = message.
                    };
                    //Заносим изменения в бд
                    _ = RequestApi<CoolApiModels.Messages.MessageDetails,CoolApiModels.Messages.NewMessageDetails>.Post(newMessage, $"/api/Messages/{message.Message.Id}", message.Token);
                    break;
            }
        }

        public override void SendData(Transfer data)
        {
            base.SendData(data);
      
        }
 
    }
}