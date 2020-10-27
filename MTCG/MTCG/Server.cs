using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using MTCG.Server;

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
            server.Start(1);
            StartListener();
        }

        public void StartListener()
        {
            try{
                while (true){
                    Console.WriteLine("Waiting for a connection..." + Thread.CurrentThread.ManagedThreadId);
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    Thread t = new Thread((HandleDeivce));
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
            try{
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0){
                    data = Encoding.ASCII.GetString(bytes, 0, i);

                    string response = req.Context(data, messages);

                    AnswerClient(stream, response);
                }
            }catch (Exception e){
                Console.WriteLine("Exception: {0}", e.ToString());
                client.Close();
            }
        }

        void AnswerClient(NetworkStream stream, string response)
        {
            string answerString =
                        "HTTP/1.1 200 OK \n" +
                        "Server: MTCG \n" +
                        "Content-Length:" + response.Length + " \n" +
                        "Content-Language: de \n" +
                        "Connection: close \n" +
                        "Keep-Alive: timeout=50, max=0 \n" +
                        "Access-Control-Allow-Origin: *\n" +
                        "Access-Control-Allow-Credentials: true\n" +
                        "Content-Type: text/plain\n" +
                        "\n" + response;

            Byte[] reply = Encoding.ASCII.GetBytes(answerString);
            stream.Write(reply, 0, reply.Length);
            stream.Flush();
            Console.WriteLine("{1}: Sent: {0}", answerString, Thread.CurrentThread.ManagedThreadId);
        }
    }
}

