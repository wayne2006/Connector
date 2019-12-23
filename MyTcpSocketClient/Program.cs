using Connector.DotNettySocket.Protocol;
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
            int i = 0;
            while (true)
            {
                i++;
                //await client.SendAsync(req.ToString());
                Console.WriteLine(i.ToString());
                await Task.Delay(1000);
            }

            Console.ReadLine();
        }
    }
}
