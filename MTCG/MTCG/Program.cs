using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
//using MTCG.Server;

namespace MTCG.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            List<Thread> clients = new List<Thread>();
            Thread t = new Thread(delegate ()
            {
                Server myserver = new Server("127.0.0.1", 8000);
                
            });
            t.Start();
            clients.Add(t);

            Console.WriteLine("Server Started...!");
            
        }
    }
}
