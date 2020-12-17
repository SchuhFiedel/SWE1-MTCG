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

            Server myserver = new Server("127.0.0.1", 8000);

            Console.WriteLine("Server Started...!");
            
        }
    }
}
