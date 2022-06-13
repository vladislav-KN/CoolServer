using CoolApiModels.Users;
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
    public abstract class ConnectionHandler : IConnection
    {

        internal string jsonString;
        internal int port;
        public int Port { get => port; }
        public virtual void SendData(Transfer data)
        {
            jsonString = JsonConvert.SerializeObject(data);
        }

        
    }

    public class TcpConnections
    {
        private IDictionary<Guid, TcpClient> tcpUsers;

        public TcpConnections()
        {
            tcpUsers = new Dictionary<Guid,TcpClient>();
        }
        public IDictionary<Guid, TcpClient> TcpUsers { get { return tcpUsers; } }

        readonly object iloc = new object();

        internal List<TcpClient> FindUser(IEnumerable<UserDetails> users)
        {
            List<TcpClient> tcpClients = new List<TcpClient>();
            foreach(UserDetails user in users)
            {
                tcpClients.Add(tcpUsers.Where(x=>x.Key == user.Id).Select(x=>x.Value).FirstOrDefault());
            }

            return tcpClients;
        }

        internal void Add(Guid id, TcpClient client)
        {
            lock (iloc)
            {
                tcpUsers.Add(id, client);
            }
        }

        internal void Remove(TcpClient cl)
        {
            lock (iloc)
            {
                tcpUsers.Remove(tcpUsers.Where(x => x.Value == cl).Select(x => x.Key).FirstOrDefault());
            }
            
        }
    }

    public class TcpConnectionHandler: ConnectionHandler
    {

        TcpConnections connection;
        public TcpConnectionHandler()
        {
            port = 30000;
            connection = new TcpConnections();

        }
        public TcpConnectionHandler(int p)
        {
            port = p;
            connection = new TcpConnections();

        }
         

        public async Task HandlerMessage(TcpClient client)
        {
            var stream = client.GetStream();
            var binf = new BinaryFormatter();
            var message = JsonConvert.DeserializeObject(binf.Deserialize(stream) as string) as TransferMessages;
            if (!connection.TcpUsers.ContainsKey(message.Message.Sender.Id))
            {
                connection.Add(message.Message.Sender.Id, client);
            }
            switch (message.Action)
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
                    if (message.ForAll is null)
                        message.ForAll = false;
                    _ = RequestApi<object,object>.Delete($"/api/Messages/{message.Message.Id}?IsForAll={message.ForAll}", message.Token);
                     
                    break;
                case ACTION.SEND:
                    CoolApiModels.Messages.NewMessageDetails newMessage = new CoolApiModels.Messages.NewMessageDetails()
                    {
                        ChatId = message.Message.ChatId,
                        Attachments = message.Message.Attachments,
                        Text = message.Message.Text
                    };
                    //Заносим изменения в бд
                    _ = RequestApi<CoolApiModels.Messages.MessageDetails,CoolApiModels.Messages.NewMessageDetails>.Post(newMessage, $"/api/Messages/{message.Message.Id}", message.Token);
                    break;

            }
            var chat = await RequestApi<CoolApiModels.Chats.ChatDetails, int>.Get($"/api/Chats/{message.Message.ChatId}", message.Token);
            SendMessage(message, connection.FindUser(chat.ChatMembers.ToList().Select(x=>x).Where(x=>x.Id!=message.Message.Sender.Id)));
        }

 
        public void SendMessage(Transfer data, List<TcpClient> clients)
        {
            base.SendData(data);
            foreach(TcpClient cl in clients)
            {
                if (cl.Connected)
                {
                    var stream = cl.GetStream();
                    var binf = new BinaryFormatter();
                    binf.Serialize(stream, jsonString);
                }
                else
                {
                    connection.Remove(cl);
                }
                
            }
        }
 
    }
}