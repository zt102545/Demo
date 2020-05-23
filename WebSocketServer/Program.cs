using Common;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            #region WebSocketTest
            Thread t = new Thread(WSTestSend.Start);
            t.Start();

            int i = 0;
            while (true)
            {
                Thread.Sleep(5000);
                i++;
                string str = $"Send i:{i}";
                WSTestSend.Send(str);
                Console.WriteLine(str);
            }
            #endregion
        }
    }

}
