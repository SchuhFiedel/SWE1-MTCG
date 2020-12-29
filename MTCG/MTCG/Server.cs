using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace MTCG.Server
{
    public class Server
    {
        TcpListener server = null;
        List<string> messages = new List<string>();

        public Server(string ip, int port)
        {
            IPAddress localAddr = IPAddress.Parse(ip);
            server = new TcpListener(localAddr, port);
            server.Start(5);
            StartListener();
        }

        public void StartListener()
        {
            try{
                while (true) //run until button is pressed
                {
                    Console.WriteLine("Waiting for a connection..." + Thread.CurrentThread.ManagedThreadId);
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    Thread t = new Thread((HandleDeivce));
                    if(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                    t.Start(client);
                    
                }
            }catch (SocketException e){
                Console.WriteLine("SocketException: {0}", e);
                server.Stop();
            }
        }

        public void HandleDeivce(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();
            RequestContext req = new RequestContext();
            string data = null;
            Byte[] bytes = new Byte[200000];
            int i;
            //run thread with connection to single client until client ends connection or Error happens
            bool stop = true;
            while (stop)
            {
                try
                {
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = Encoding.ASCII.GetString(bytes, 0, i);

                        Tuple<string, string> response = req.Context(data, messages); //send StreamData to the Request Kontext 
                        if (response.Item2 == "EXIT") stop = false;                   // Response = Tuple (Response, ConnStatus)
                        AnswerClient(stream, response.Item1);
                        if(stop == false) { break; }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: {0}", e.ToString());
                    stream.Close();
                    client.Close();
                    stop = false;
                }

                data = null;
            }
            stream.Close();
            client.Close();
        }

        void AnswerClient(NetworkStream stream, string response)
        {
            string answerString =
                        "HTTP/1.1 200 OK \n" +
                        "Server: MTCG \n" +
                        "Content-Length: " + response.Length + " \n" +
                        "Content-Language: de \n" +
                        "Connection: open \n" +
                        "Keep-Alive: timeout=50, max=0 \n" +
                        "Access-Control-Allow-Origin: *\n" +
                        "Access-Control-Allow-Credentials: true\n" +
                        "Content-Type: application/json\n" +
                        "\n" + response;

            Byte[] reply = Encoding.ASCII.GetBytes(answerString);
            stream.Write(reply, 0, reply.Length);
            stream.Flush();
            Console.WriteLine("{1}: Sent:\n {0}", answerString, Thread.CurrentThread.ManagedThreadId);
        }
    }
}

