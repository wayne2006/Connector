using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTcpSocket.Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await TelnetServer.RunAsync();

            Console.ReadLine();
        }
    }
}
