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
            Int32 port = 8000;
            string address = "127.0.0.1";
            string uri = "http://" + address + ":" + port;
            //string altUri = "https://httpbin.org/";
            HttpClient client = new HttpClient();

            
            TcpClient t_client = new TcpClient(address, port);
            NetworkStream stream = t_client.GetStream();
            //t_client.Client.

            string input;

            do
            {
                PrintMenu();
                input = Console.ReadLine();

                switch (input)
                {
                    case "EXIT":
                        Console.WriteLine("Ending Program!");
                        //HttpRequest("EXIT", uri, stream);
                        break;
                    case "1":
                        Console.WriteLine("Get ALL Messages!");
                        //await GetAllMessages(client, uri);
                        HttpRequest("GET1", uri, stream);

                        break;
                    case "2":
                        Console.WriteLine("Get Specific Message!");
                        //await GetMessage(client, uri);
                        HttpRequest("GET2", uri, stream);

                        break;
                    case "3":
                        Console.WriteLine("Post Message!");
                        //await PostMessage(client, uri);
                        HttpRequest("POST", uri, stream);

                        break;
                    case "4":
                        Console.WriteLine("Put Message!");
                        //await PutMessage(client, uri);
                        HttpRequest("PUT", uri, stream);

                        break;
                    case "5":
                        Console.WriteLine("Delete message!");
                        //await DelMessage(client, uri);
                         HttpRequest("DELETE", uri, stream);

                        break;
                    default:
                        break;
                }

                // Bytes Array to receive Server Response.
                Byte[] data = new Byte[256];
                String response = String.Empty;
                // Read the Tcp Server Response Bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", response);
                //Thread.Sleep(2);


            } while(input != "EXIT");
            Console.ReadKey();
        }


        public static void PrintMenu()
        {
            Console.WriteLine("\n\n");
            Console.WriteLine("***************************************************************************");
            Console.WriteLine("1) Get all messages");
            Console.WriteLine("2) Get one messages");
            Console.WriteLine("3) Send and save message on the Server");
            Console.WriteLine("4) Update message (overwrite)");
            Console.WriteLine("5) Delete message");
            Console.WriteLine("EXIT) Close The Program");
            Console.WriteLine("***************************************************************************");
            Console.WriteLine("\n");
        }

        static void HttpRequest(string reqType, string uri, NetworkStream stream)
        {
            string  message = "";
            string path = "/messages";

            if(reqType == "GET1") { reqType = "GET"; }
            else if(reqType == "GET2"){
                reqType = "GET";
                Console.WriteLine("Please Enter Message ID!");
                path += "/" + Console.ReadLine();
            }
            else if (reqType == "POST"){
                Console.WriteLine("Please Enter new Message!");
                message = Console.ReadLine();
            }
            else if (reqType == "PUT"){
                Console.WriteLine("Please Enter Message ID!");
                path += "/" + Console.ReadLine();
                Console.WriteLine("Please Enter new Message");
                message = Console.ReadLine();
            }
            else if (reqType == "DELETE"){
                Console.WriteLine("Please Enter Message ID!");
                path += "/" + Console.ReadLine();
            }
            
            string answerString = "";
            if (message.Length == 0){
                answerString =
                        reqType + " " + path +" "+ "HTTP/1.1\n" +
                        "Host: " + uri + "\n" +
                        "Connection: keep-alive \n" +
                        "Keep-Alive: timeout=50, max=0 \n" +
                        "Access-Control-Allow-Origin: *\n" +
                        "Access-Control-Allow-Credentials: true\n" +
                        "Content-Type: text/plain; charset=utf-8\n";
            }else{
                answerString =
                        reqType + " " + path + " " + "HTTP/1.1\n" +
                        "Host: " + uri + "\n" +
                        "Content-Length:" + message.Length + " \n" +
                        "Content-Language: de \n" +
                        "Connection: keep-alive \n" +
                        "Keep-Alive: timeout=50, max=0 \n" +
                        "Access-Control-Allow-Origin: *\n" +
                        "Access-Control-Allow-Credentials: true\n" +
                        "Content-Type: text/plain; charset=utf-8\n" +
                        "\n" + message;
            }
            Byte[] reply = Encoding.ASCII.GetBytes(answerString);
            stream.Write(reply, 0, reply.Length);
            stream.Flush();
            Console.WriteLine("Sent: \n {0}", answerString);
        }
    }
}



/*
        static async Task GetAllMessages(HttpClient client, string uri)
        {
            string getStringReturn = await client.GetStringAsync(uri + "/messages");
            Console.WriteLine(getStringReturn);
        }

        static async Task GetMessage(HttpClient client, string uri)
        {
            Console.WriteLine("Pleas Enter message ID!");
            string input = Console.ReadLine();
            string getStringReturn = await client.GetStringAsync(uri + "/messages/" + input);
            Console.WriteLine(getStringReturn);
        }

        static async Task PostMessage(HttpClient client, string uri)
        {
            Console.WriteLine("Pleas Enter message!");
            string input = Console.ReadLine();
            HttpContent data = new StringContent(input, Encoding.UTF8, "text/plain");
            HttpResponseMessage result = await client.PostAsync(uri + "/messages", data);
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
            HttpResponseMessage result = await client.PutAsync(uri + "/messages/" + id, data);
            string s_result = result.Content.ReadAsStringAsync().Result;
            Console.WriteLine(s_result);
        }

        static async Task DelMessage(HttpClient client, string uri)
        {
            Console.WriteLine("Please Enter message ID!");
            string id = Console.ReadLine();
            HttpResponseMessage result = await client.DeleteAsync(uri + "/messages/" + id);
            string s_result = result.Content.ReadAsStringAsync().Result;
            Console.WriteLine(s_result);
        }
*/


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
