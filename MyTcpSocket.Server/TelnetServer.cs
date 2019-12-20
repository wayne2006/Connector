using Coldairarrow.DotNettySocket;
using Coldairarrow.DotNettySocket.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTcpSocket.Server
{
    public static class TelnetServer
    {
        static ITcpSocketServer tcpSocketServer;
        public static async Task RunAsync()
        {
            var tcpServerBuild = SocketBuilderFactory.GetTcpSocketServerBuilder(6001)
                .SetLengthFieldEncoder(2)
                .SetLengthFieldDecoder(ushort.MaxValue, 0, 2, 0, 2);

            tcpServerBuild.OnConnectionClose((server, connection) =>
            {
                Console.WriteLine($"连接关闭,连接名[{connection.ConnectionName}],当前连接数:{server.GetConnectionCount()}");
            })
                .OnException(ex =>
                {
                    Console.WriteLine($"服务端异常:{ex.Message}");
                })
                .OnNewConnection((server, connection) =>
                {
                    connection.ConnectionName = $"名字{connection.ConnectionId}";
                    Console.WriteLine($"新的连接[{connection.ClientAddress.Address.MapToIPv4().ToString()}]:{connection.ConnectionName},当前连接数:{server.GetConnectionCount()}");

                })
                .OnRecieve((server, connection, bytes) =>
                {
                    Console.WriteLine($"服务端:数据{Encoding.UTF8.GetString(bytes)}");
                    StringRequestInfo stringRequestInfo = StringRequestInfo.FromBytes(bytes);
                    Console.WriteLine($"服务端:KEY:{stringRequestInfo.Key} BODY:{stringRequestInfo.Body} FirstParam:{stringRequestInfo.GetFirstParam()}");
                    //connection.Send(bytes);
                })
                .OnSend((server, connection, bytes) =>
                {
                    Console.WriteLine($"向连接名[{connection.ConnectionName}]发送数据:{Encoding.UTF8.GetString(bytes)}");
                })
                .OnServerStarted(server =>
                {
                    Console.WriteLine($"服务已启动 监听端口:{server.Port}");
                });

            tcpSocketServer = await tcpServerBuild.BuildAsync();
        }
    }
}
