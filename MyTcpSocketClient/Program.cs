using Coldairarrow.DotNettySocket.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTcpSocketClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new TelnetClient();
            client.Run();

            var req = new StringRequestInfo("Echo", "123", null);
            while (true)
            {
                await client.SendAsync(req.ToString());

                await Task.Delay(100);
            }

            Console.ReadLine();
        }
    }
}
