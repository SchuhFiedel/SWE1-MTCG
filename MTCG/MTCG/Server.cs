using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MTCG;

namespace MTCG.Server
{
    public class Server
    {
        TcpListener server = null;

        public Server(string ip, int port)
        {
            IPAddress localAddr = IPAddress.Parse(ip);
            server = new TcpListener(localAddr, port);
            server.Start(1);
            StartListener();
        }
        public void StartListener()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for a connection..." + Thread.CurrentThread.ManagedThreadId);
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    Thread t = new Thread((HandleDeivce));
                    t.Start(client);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                server.Stop();
            }
        }
        public void HandleDeivce(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();
            string data = null;
            Byte[] bytes = new Byte[200000];
            int i;
            try
            {
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("{1}: Received: {0}", data, Thread.CurrentThread.ManagedThreadId);

                    //StringReader reader = new StringReader(data);
                    //string messageLineOne = reader.ReadLine();

                    string[] tokens = data.Split(' ');
                    Console.WriteLine("LENGTH = " + tokens.Length);
                    string request = tokens[0];
                    string path = tokens[1];

                    tokens = path.Split('/');
                    int counter = 0;
                    foreach (string x in tokens){
                        Console.WriteLine(counter +"=" +x);
                        counter++;
                    }
                    


                    //Console.WriteLine("Token 1: " + request + " Token 2: " + path);
                    Console.WriteLine("\n\n");

                    string response = "";

                    switch (request)
                    {
                        case "GET":
                            if(tokens[1] == "messages" && tokens.Length ==3){
                                //send specific message
                                response = "SPECIFIC MESSAGE" + tokens[1] + " " + tokens[2];
                            }
                            else if(tokens[1] == "messages" && tokens.Length <= 2)
                            {
                                //send all message
                                response = "ALL MESSAGES" + tokens[1];
                            }
                            else
                            {
                                response = "ERROR - wron number of arguments";
                            }
                            break;
                        case "PUT":
                            break;
                        case "DELETE":
                            break;
                        case "POST":
                            break;
                        default:
                            response = "ERROR";
                            break;
                    }

                    
                    answerClient(stream, response);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
                client.Close();
            }
        }

        void answerClient(NetworkStream stream, string response)
        {
            
            string answerString =
                        "HTTP/1.1 200 OK \n" +
                        "Server: MTCG \n" +
                        "Content-Length:" + response.Length + " \n" +
                        "Content-Language: de \n" +
                        "Connection: Keep-Alive \n" +
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

