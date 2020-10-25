using System;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net.Http;
using MTCG;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string adress = "127.0.0.1";
            string uri = "http://" + adress + ":8000/";
            //string altUri = "https://httpbin.org/";
            HttpClient client = new HttpClient();

            for (int i = 10; i > 0; i--)
            {
                string message = Console.ReadLine();
                HttpContent data = new StringContent(message, Encoding.UTF8, "text/plain");

                //HttpResponseMessage postReturn = await client.PostAsync(uri, data);
                //string result = postReturn.Content.ReadAsStringAsync().Result;
                //var send = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, "https://httpbin.org/post"));
                HttpResponseMessage getReturn = await client.GetAsync(uri+"messages/5");
                //string getStringReturn = await client.GetStringAsync(uri);
                //Console.WriteLine(postReturn);
                //Console.WriteLine(result);
                Console.WriteLine(getReturn);
                // Console.WriteLine(getStringReturn);

                //HttpResponseMessage con = await client.DeleteAsync(uri+"messages/" + "1");
                //Console.WriteLine(con);

                Console.ReadKey();
            }
        }


    }
}




/*
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
 }*/
