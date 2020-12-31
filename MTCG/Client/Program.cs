using System;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;
using MTCG;

namespace Client
{
    public class Program
    {
        public static void Main ()
        {
            Int32 port = 8000;
            string address = "127.0.0.1";
            string uri = "http://" + address + ":" + port;
            Console.BufferWidth = 100;
            Console.SetWindowSize(Console.BufferWidth, 80);
            //Console.WriteLine(System.Reflection.Assembly.GetEntryAssembly().Location);

            TcpClient t_client = new TcpClient(address, port);
            NetworkStream stream = t_client.GetStream();
            ClientRequestHandler httpHandler = new ClientRequestHandler(t_client, stream);

            string authenticationToken = "";
            bool loggedIn = false;

            int input = 0;
            do
            {
                PrintMenu(loggedIn);
                if(input != 12) //input is automatically set to 12 if in battle mode
                {
                    input = Int32.Parse(Console.ReadLine());
                    if (loggedIn == false && input > 2 && input != 20)
                    {
                        Console.WriteLine("Please Log in or Register First!");
                        input = 0;
                    }
                }
                switch (input)
                {
                    case 1:
                        //DONE
                        Console.WriteLine("Register new User");
                        httpHandler.HttpRequest(1, uri, stream, authenticationToken, loggedIn);
                        break;
                    case 2:
                        //DONE
                        Console.WriteLine("Login User!");
                        httpHandler.HttpRequest(2, uri, stream, authenticationToken, loggedIn);
                        break;
                    case 3:
                        //DONE
                        Console.WriteLine("GET All User Info!");
                        httpHandler.HttpRequest(3, uri, stream, authenticationToken, loggedIn);
                        break;
                    case 4:
                        //DONE
                        Console.WriteLine("Update User Info");
                        httpHandler.HttpRequest(4, uri, stream, authenticationToken, loggedIn);
                        break;
                    case 5:
                        //DONE
                        Console.WriteLine("Buy More Coins");
                        httpHandler.HttpRequest(5, uri, stream, authenticationToken, loggedIn);
                        break;
                    case 6:
                        //DONE
                        Console.WriteLine("Buy CardPackages");
                        httpHandler.HttpRequest(6, uri, stream, authenticationToken, loggedIn);
                        break;
                    case 7:
                        //DONE
                        Console.WriteLine("Show all owned Cards");
                        httpHandler.HttpRequest(7, uri, stream, authenticationToken, loggedIn);
                        break;
                    case 8:
                        //DONE
                        Console.WriteLine("Show User Deck");
                        httpHandler.HttpRequest(8, uri, stream, authenticationToken, loggedIn);
                        break;
                    case 9:
                        //DONE
                        Console.WriteLine("Put Card In Deck");
                        httpHandler.HttpRequest(9, uri, stream, authenticationToken, loggedIn);
                        break;
                    case 10:
                        //DONE
                        Console.WriteLine("Put Card In Deck");
                        httpHandler.HttpRequest(10, uri, stream, authenticationToken, loggedIn);
                        break;
                    case 11:
                        //TO-DO show all tradings = GET tradings; trade = POST tradings, delete deal = DELETE tradings/tradeID
                        break;
                    case 12:
                        //TO-DO go to matchmaking and fight = POST battles
                        Console.WriteLine("Start Battle");
                        httpHandler.HttpRequest(12, uri, stream, authenticationToken, loggedIn);
                        break;
                    case 20:
                        Console.WriteLine("Logout And Close Program");
                        httpHandler.HttpRequest(20, uri, stream, authenticationToken, loggedIn);
                        t_client.Close();
                        break;
                    default:
                        Console.WriteLine("Invalid Input!");
                        break;
                }
                if (t_client.Connected)
                {
                    // Bytes Array to receive Server Response.
                    Byte[] data = new Byte[200000];
                    String response = String.Empty;
                    // Read the Tcp Server Response Bytes.
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                    string[] messageLines = response.Split('\n');
                    string[] tokens = messageLines[0].Split(' '); // first row
                    int rowcounter = SearchDataRow(messageLines); // find row where Header ends
                    Tuple<string[], string> allData = SplitData(rowcounter, messageLines); // split messageLines in HeaderArray and Payload string
                    Dictionary<string, string> headers = GetHeaders(allData.Item1); //Split Header Array into Dictinary with Attributes and Values

                    if (!loggedIn)
                    {
                        Tuple<bool, string> auth = CheckAuthenticationData(allData.Item2);
                        loggedIn = auth.Item1;
                        authenticationToken = auth.Item2;
                    }
                    if (InBattleMode(allData.Item2))
                    {
                        input = 12;
                    }
                    else
                    {
                        input = 0;
                    }

                    Console.WriteLine("Received:\n{0} \n\n", response);
                    //Thread.Sleep(2);
                }
            } while(input != 20);
            Console.ReadKey();
        }

        //search for row in which the HTTP Payload is
        private static int SearchDataRow(string[] messageLines)
        {
            int rowcounter = 0;
            foreach (string x in messageLines)
            {
                rowcounter += 1;
                if (x == "\r" || x == "\n" || x == "") // if any kind of new line, break
                {
                    break;
                }
            }
            return rowcounter;
        }

        //Make Json format strings more readable
        private static string RemoveUnnecessaryChars(string info)
        {
            info = info.Replace("\n", "").Replace("\r", "").Replace("\t", "")
                        .Replace("{", "").Replace("}", "").Replace(" \"", "").Replace("\"", "");
            return info;
        }

        //Split data into Headers and Payload
        private static Tuple<string[], string> SplitData(int rowcounter, string[] messageLines)
        {
            string[] a = new string[] { };
            string b = "";
            List<string> tmp = new List<string>();

            for (int i = 0; i < rowcounter - 1; i++)
            {
                tmp.Add(messageLines[i].Replace(" ", ""));
            }
            for (int i = 0; i + rowcounter < messageLines.Length; i++)
            {
                b += messageLines[i + rowcounter];
            }

            a = tmp.ToArray();
            Tuple<string[], string> addMessage = Tuple.Create(a, b);

            return addMessage;
        }

        //get all headers into a dictionary
        private static Dictionary<string, string> GetHeaders(string[] data)
        {
            Dictionary<string, string> Headers = new Dictionary<string, string>();

            for (int i = 1; i < data.Length - 1; i++)
            {
                string[] tmp = data[i].Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "").Split(":");
                Headers.Add(tmp[0], tmp[1]);
            }
            return Headers;
        }

        //set  authentication Token which is received from the server
        private static Tuple<bool,string> CheckAuthenticationData(string info)
        {
            info = RemoveUnnecessaryChars(info);
            string[] attr = info.Split(',');

            bool success = false;
            string token = "";
            for (int i = 0; i < attr.Length; i++)
            {
                string[] rowinfo = attr[i].Split(':');
                if (rowinfo[0] == "Login" && rowinfo[1] == "Success") { success = true; }
                if (rowinfo[0] == "Token") { token = rowinfo[1]; }
            }

            return Tuple.Create(success,token);

        }

        public static void PrintMenu(bool isLoggedIn)
        {
            
            Console.WriteLine("\n\n");
            Console.WriteLine("***************************************************************************");
            Console.WriteLine("1 ) Register new User");
            Console.WriteLine("2 ) Login User");
            if (isLoggedIn)
            {
                Console.WriteLine("3 ) Get All User Infos (User Score)");
                Console.WriteLine("4 ) Update User Information");
                Console.WriteLine("5 ) Buy Coins");
                Console.WriteLine("6 ) Buy CardPackages");
                Console.WriteLine("7 ) Show all owned Cards");
                Console.WriteLine("8 ) Show your current Deck");
                Console.WriteLine("9 ) Add Owned Cards to your Deck");
                Console.WriteLine("10) Show Score-Board");
                Console.WriteLine("11) Trading Menu");
                Console.WriteLine("12) Start Matchmaking");
            }
            Console.WriteLine("20) Logout and Close Program");
            Console.WriteLine("***************************************************************************");
            Console.WriteLine("\n");
        }

        public static bool InBattleMode(string info)
        {
            info = RemoveUnnecessaryChars(info);
            string[] attr = info.Split(',');

            bool success = false;
            for (int i = 0; i < attr.Length; i++)
            {
                string[] rowinfo = attr[i].Split(':');
                if (rowinfo[0] == "Matchmaking" && rowinfo[1] != "Unsuccessful") { success = true; }
                if (rowinfo[0] == "Battle" && rowinfo[1] != "Over") { success = false; }
            }

            return success;
        }
    }
}
