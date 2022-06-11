using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CoolServer.Interfaces;

namespace CoolServer.MessageTransfer
{
    
    public class MultiTask<T>:IHostedService, IDisposable
        where T : Connection
    {
         
        private Timer timer;
        T connection;
        public MultiTask()
        {
            connection = default(T);
        }

        public MultiTask(string settings)
        {
            connection = default(T);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {

            try
            {
                IPAddress ip;
                if (!IPAddress.TryParse("127.0.0.1", out ip))
                {
                    Console.WriteLine("Failed to get IP address, service will listen for client activity on all network interfaces.");
                    ip = IPAddress.Any;
                }
                
                var tcpListener = new TcpListener(ip, connection.Port);

                tcpListener.Start();

                var task = tcpListener.AcceptTcpClientAsync();

                var tasks = new List<Task<TcpClient>>();
                tasks.Add(task);
                Task<TcpClient> tcpClientTask;

                while ((tcpClientTask = await Task.WhenAny(tasks)) != null)
                {
                    var tcpListenerPortPair = tasks.First();
                    var port = connection.Port;


                    tasks.Remove(tcpClientTask);

                    // This needs to be async. What to do with its Task?
                    // It cannot be awaited here.
                    var handlerTask = connection.Handler(tcpClientTask.Result);

                    var new_task = tcpListener.AcceptTcpClientAsync();

                    tasks.Add(new_task);
                }
            }
            catch (Exception e)
            {
                 
            }
        }

        //private async Task HandleTcpConnection(TcpClient client, int port)
        //{

        //    var stream = client.GetStream();
        //    var binf = new BinaryFormatter();
        //    var message = binf.Deserialize(stream) as TransferMessages;
        //    switch (message.action)
        //    {
        //        case ACTION.CHNG:
        //            //Заносим изменения в бд
        //            CoolApiModels.Messages.MessageNewDetails newMessage = new CoolApiModels.Messages.MessageNewDetails() { 
        //                Text = message.Message.Text,
        //                Attachments = message.Message.Attachments,
        //                IsViewed = message.Message.IsViewed
        //            };
        //            _ = RequestApi<CoolApiModels.Messages.MessageNewDetails>.Put(newMessage, $"/api/Messages/{message.Message.Id}", message.token);
        //            break;
        //        case ACTION.DEL:
        //            //Заносим изменения в бд
        //            break;
        //        case ACTION.SEND:
        //            //Заносим изменения в бд
        //            break;
        //    }
             
        //    //перенаправляем само сообщение пользователю если он есть в списке

        //}

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
