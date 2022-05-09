using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace CoolServer.MessageTransfer
{
    public class TcpMultiTask:IHostedService, IDisposable
    {
        private Timer timer;
        TcpConnection connection;
        TcpMultiTask(string config)
        {

        }
        TcpMultiTask()
        {

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        string str = Console.ReadLine();
                        if (str == "addadmin")
                        {
                            using (var con = new SqlConnection())
                            {
                                con.ConnectionString = connectionString;
                                con.Open();
                                Console.WriteLine("Введите имя пользователя");
                                string log = Console.ReadLine();
                                string strSQL = @"INSERT INTO Users ([Login], [pasword], [Admin]) values(@log, @pas, 1)";
                                var cmd = new SqlCommand(strSQL, con);
                                cmd.Parameters.AddWithValue("@log", log);
                                cmd.Parameters.Add("@pas", SqlDbType.Int);
                                Console.WriteLine("Введите пароль");

                                string pass = Console.ReadLine();

                                cmd.Parameters["@pas"].Value = User.StringHashCode20(pass);

                                cmd.ExecuteNonQuery();
                                Console.WriteLine("Успешно добавлено");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            });
            try
            {
                IPAddress ip;
                if (!IPAddress.TryParse("127.0.0.1", out ip))
                {
                    Console.WriteLine("Failed to get IP address, service will listen for client activity on all network interfaces.");
                    ip = IPAddress.Any;
                }
                IDictionary<Task<TcpClient>, Tuple<int, TcpListener>> tcpListeners = new Dictionary<Task<TcpClient>, Tuple<int, TcpListener>>();

                foreach (int port in Enum.GetValues(typeof(Ports)))
                {
                    var tcpListener = new TcpListener(ip, port);

                    tcpListener.Start();

                    var task = tcpListener.AcceptTcpClientAsync();
                    var tcpListenerPortPair = new Tuple<int, TcpListener>(port, tcpListener);

                    tcpListeners.Add(task, tcpListenerPortPair);
                }

                Task<TcpClient> tcpClientTask;

                while ((tcpClientTask = await System.Threading.Tasks.Task.WhenAny(tcpListeners.Keys)) != null)
                {
                    var tcpListenerPortPair = tcpListeners[tcpClientTask];
                    var port = tcpListenerPortPair.Item1;
                    var tcpListener = tcpListenerPortPair.Item2;

                    tcpListeners.Remove(tcpClientTask);

                    // This needs to be async. What to do with its Task?
                    // It cannot be awaited here.
                    var handlerTask = HandleByPortNumber(tcpClientTask.Result, port);

                    var task = tcpListener.AcceptTcpClientAsync();

                    tcpListeners.Add(task, tcpListenerPortPair);
                }
            }
            catch (Exception e)
            {
                Log.Info(DateTime.Now.ToString("MM/dd/yyyy h:mm tt") + " Ошибка " + e.Message);
            }
        }
    }

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
