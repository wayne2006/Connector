using Connector.DotNettySocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyTcpSocketClient
{
    
    public class TelnetClient
    {
        private ITcpSocketClient tcpSocketClient;
        private Timer _timer;

        public TelnetClient()
        {
            
        }

        public void Run()
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }
        private void DoWork(object state)
        {
            if (tcpSocketClient != null && !tcpSocketClient.GetStatus())
            {
                Console.WriteLine($"客户端开始重连");
                ConnectAsync().Wait();
            }
            if (tcpSocketClient == null)
            {
                Console.WriteLine($"客户端开始连接");
                ConnectAsync().Wait();
            }
        }

        private async Task ConnectAsync()
        {
            var tcpClientBuild = SocketBuilderFactory.GetTcpSocketClientBuilder("127.0.0.1", 6001)
                .SetLengthFieldEncoder(2)
                .SetLengthFieldDecoder(ushort.MaxValue, 0, 2, 0, 2);

            tcpClientBuild.OnClientStarted(client =>
            {
                Console.WriteLine($"客户端启动{client.Ip} {client.Port}");
            })
                .OnClientClose(client =>
                {
                    Console.WriteLine($"客户端关闭");
                })
                .OnException(ex =>
                {
                    Console.WriteLine($"异常:{ex.Message}");
                })
                .OnRecieve((client, bytes) =>
                {
                    Console.WriteLine($"客户端:收到数据:{Encoding.UTF8.GetString(bytes)}");
                })
                .OnSend((client, bytes) =>
                {
                    Console.WriteLine($"客户端:发送数据:{Encoding.UTF8.GetString(bytes)}");
                });

            try
            {
                tcpSocketClient = await tcpClientBuild.BuildAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"连接异常：{ex.Message}");
            }
        }

        public async Task SendAsync(string msg)
        {
            if (tcpSocketClient != null)
            {
                await tcpSocketClient.Send(msg);
            }
        }
    }
}
