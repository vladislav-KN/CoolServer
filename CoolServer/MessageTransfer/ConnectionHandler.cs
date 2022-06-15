using CoolApiModels.Messages;
using CoolApiModels.Users;
using CoolServer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using TransferLibrary;

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
                var add = tcpUsers.Where(x => x.Key == user.Id).Select(x => x.Value).FirstOrDefault();
                if (add is null)
                    continue;
                tcpClients.Add(add);
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
            var message =  binf.Deserialize(stream) as TransferMessages;
            if(message is null)
            if (!connection.TcpUsers.ContainsKey(message.Message.Sender.Id))
            {
                connection.Add(message.Message.Sender.Id, client);
            }
            if(!string.IsNullOrEmpty(message.Token)) 
            { 
                Tuple<MessageDetails,HttpStatusCode, ProblemDetails> messageCU = null;
                Tuple< HttpStatusCode, ProblemDetails> messageD = null;
                switch (message.Action)
                {
                    case ACTION.CHNG:
                        //Заносим изменения в бд
                        MessageNewDetails messageNew = new MessageNewDetails()
                        {
                            Text = message.Message.Text,
                            Attachments = message.Message.Attachments,
                            IsViewed = message.Message.IsViewed
                        };
                        messageCU = await RequestApi<MessageDetails, MessageNewDetails>.Put(messageNew, $"Messages/{message.Message.Id}", message.Token);
                        break;
                    case ACTION.DEL:
                        //Заносим изменения в бд
                        if (message.ForAll is null)
                            message.ForAll = false;
                        messageD = await RequestApi<object,object>.Delete($"Messages/{message.Message.Id}?IsForAll={message.ForAll}", message.Token);
                     
                        break;
                    case ACTION.SEND:
                        NewMessageDetails newMessage = new NewMessageDetails()
                        {
                            ChatId = message.Message.ChatId,
                            Attachments = message.Message.Attachments,
                            Text = message.Message.Text
                        };
                        //Заносим изменения в бд
                        messageCU = await RequestApi<MessageDetails,NewMessageDetails>.Post(newMessage, $"Messages", message.Token);
                        break;

                }
                var chat = await RequestApi<CoolApiModels.Chats.ChatDetails, int>.Get($"Chats/{message.Message.ChatId}", message.Token);
                if(!(messageCU is null))
                    if(!(messageCU.Item1 is null))
                        message.Message.Id = messageCU.Item1.Id;
 
                if (chat.Item1 != null)
                    SendMessage(message, connection.FindUser(chat.Item1.ChatMembers));
            }
        }

 
        public void SendMessage(TransferMessages data, List<TcpClient> clients)
        {
            foreach(TcpClient cl in clients)
            {
                if (cl.Connected)
                {
                    var stream = cl.GetStream();
                    var binf = new BinaryFormatter();
                    binf.Serialize(stream, data);
                }
                else
                {
                    connection.Remove(cl);
                }
                
            }
        }
 
    }
}