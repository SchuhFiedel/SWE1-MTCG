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
        public static async Task Main (string[] args)
        {
            string adress = "127.0.0.1";
            string uri = "http://" + adress + ":8000/messages";
            //string altUri = "https://httpbin.org/";
            HttpClient client = new HttpClient();

            string input;

            do
            {
                PrintMenu();
                input = Console.ReadLine();

                switch (input)
                {
                    case "EXIT":
                        Console.WriteLine("Ending Program!");
                        break;
                    case "1":
                        Console.WriteLine("Get ALL Messages!");
                        await GetAllMessages(client, uri);

                        break;
                    case "2":
                        Console.WriteLine("Get Specific Message!");
                        await GetMessage(client, uri);

                        break;
                    case "3":
                        Console.WriteLine("Post Message!");
                        await PostMessage(client, uri);

                        break;
                    case "4":
                        Console.WriteLine("Put Message!");
                        await PutMessage(client, uri);

                        break;
                    case "5":
                        Console.WriteLine("Delete message!");
                        await DelMessage(client, uri);

                        break;
                    default:
                        break;
                }

                //HttpResponseMessage con = await client.DeleteAsync(uri+"messages/" + "1");
                //Console.WriteLine(con);

                
                
            }while(input != "EXIT");
            Console.ReadKey();
        }


        public static void PrintMenu()
        {
            Console.WriteLine("***************************************************************************");
            Console.WriteLine("1) Get all messages");
            Console.WriteLine("2) Get one messages");
            Console.WriteLine("3) Send and save message on the Server");
            Console.WriteLine("4) Update message (overwrite)");
            Console.WriteLine("5) Delete message");
            Console.WriteLine("EXIT) Close The Program");
            Console.WriteLine("***************************************************************************");
        }

        static async Task GetAllMessages(HttpClient client, string uri)
        {
            string getStringReturn = await client.GetStringAsync(uri);
            Console.WriteLine(getStringReturn);
        }

        static async Task GetMessage(HttpClient client, string uri)
        {
            Console.WriteLine("Pleas Enter message ID!");
            string input = Console.ReadLine();
            string getStringReturn = await client.GetStringAsync(uri + "/" + input);
            Console.WriteLine(getStringReturn);
        }

        static async Task PostMessage(HttpClient client, string uri)
        {
            Console.WriteLine("Pleas Enter message!");
            string input = Console.ReadLine();
            HttpContent data = new StringContent(input, Encoding.UTF8, "text/plain");
            HttpResponseMessage result = await client.PostAsync(uri, data);
            string s_result = result.Content.ReadAsStringAsync().Result;
            Console.WriteLine(s_result);
        }

        static async Task PutMessage(HttpClient client, string uri)
        {
            Console.WriteLine("please Enter message ID!");
            string id = Console.ReadLine();
            Console.WriteLine("Please enter message!");
            string message = Console.ReadLine();
            HttpContent data = new StringContent(message, Encoding.UTF8, "text/plain");
            HttpResponseMessage result = await client.PutAsync(uri + "/" + id, data);
            string s_result = result.Content.ReadAsStringAsync().Result;
            Console.WriteLine(s_result);
        }

        static async Task DelMessage(HttpClient client, string uri)
        {
            Console.WriteLine("Please Enter message ID!");
            string id = Console.ReadLine();
            HttpResponseMessage result = await client.DeleteAsync(uri + "/" + id);
            string s_result = result.Content.ReadAsStringAsync().Result;
            Console.WriteLine(s_result);
        }
    }
}


/*
 *  string message = Console.ReadLine();
    HttpContent data = new StringContent(message, Encoding.UTF8, "text/plain");

    HttpResponseMessage postReturn = await client.PostAsync(uri, data);
    string result = postReturn.Content.ReadAsStringAsync().Result;
    var send = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, uri));
    HttpResponseMessage getReturn = await client.GetAsync(uri + "messages/5");
    string getStringReturn = await client.GetStringAsync(uri);
    Console.WriteLine(postReturn);
    Console.WriteLine(result);
    Console.WriteLine(getReturn);
Console.WriteLine(getStringReturn);
 * /

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
