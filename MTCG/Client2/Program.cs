using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using MTCG;

namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Int32 port = 8000;
                TcpClient client = new TcpClient("127.0.0.1", port);
                NetworkStream stream = client.GetStream();
                while (true)
                {
                    string message = Console.ReadLine();
                    if (message == "0")
                    {
                        stream.Close();
                        client.Close();
                        break;
                    }
                    // Translate the Message into ASCII.
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                    // Send the message to the connected TcpServer. 
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Sent: {0}", message);
                    // Bytes Array to receive Server Response.
                    data = new Byte[256];
                    String response = String.Empty;
                    // Read the Tcp Server Response Bytes.
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("Received: {0}", response);
                    Thread.Sleep(2);
                                 
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
            //Console.Read();
        }
    }
}