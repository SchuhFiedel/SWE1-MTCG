using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Client
{
    public class ClientRequestHandler
    {
        TcpClient client;
        NetworkStream stream;

        public ClientRequestHandler(TcpClient t_client, NetworkStream stream)
        {
            client = t_client;
            this.stream = stream;
        }

        public void HttpRequest(int request, string uri, NetworkStream stream, string authentication, bool isLoggedIn)
        {
            string message = "";
            string path = "/";
            string reqType = "";

            switch (request)
            {
                case 1: // Register User 
                    //DONE
                    reqType = "POST";
                    path += "users";
                    string password = "0";
                    string password2 = "-1";

                    Console.WriteLine("Please Enter your credentials:");
                    Console.Write("Username < ");
                    string username = Console.ReadLine();

                    do
                    {
                        Console.WriteLine("Put in your password twice!");
                        Console.Write("Password < ");
                        password = Console.ReadLine();
                        Console.Write("Password again < ");
                        password2 = Console.ReadLine();
                    } while (password != password2);

                    message = "{\n" +
                              "\"Username\": \""+ username +"\",\n" +
                              "\"Password\": \"" + password + "\"\n" +
                              "}";
                    break;

                case 2: //Login 
                    //DONE
                    reqType = "POST";
                    path += "sessions";

                    Console.WriteLine("Please Enter your credentials:");
                    Console.Write("Username < ");
                    username = Console.ReadLine();
                    Console.Write("Password < ");
                    password = Console.ReadLine();

                    message = "{\n" +
                              "\"Username\": \"" + username + "\",\n" +
                              "\"Password\": \"" + password + "\"\n" +
                            "}";
                    break;

                case 3: // Get User Info 
                    //DONE
                    reqType = "GET";
                    path += "users";
                    break;

                case 4: // Update User Info
                    reqType = "PUT";
                    path += "users";
                    string bio = "-";
                    string image = "-";
                    username = "-";
                    string newUsername = "-";

                    Console.WriteLine("If you want to change information - enter data, else leave empty");
                    Console.WriteLine("Do you want to change your User Name? [Y/n]");

                    string tmp = "";
                    if (Console.ReadLine() == "Y")
                    {
                        Console.Write("Old Username < ");
                        tmp = Console.ReadLine();
                        if (tmp != "") username = tmp;
                        Console.Write("New username < ");
                        tmp = Console.ReadLine();
                        if (tmp != "") newUsername = tmp;
                    }
                    path += "/" + username;
                    Console.Write("Bio < ");
                    tmp = Console.ReadLine();
                    if (tmp != "") bio = tmp;
                    Console.Write("Image < ");
                    tmp = Console.ReadLine();
                    if (tmp != "") image = tmp;

                    message = "{\n" +
                              "\"Username\": \"" + newUsername + "\",\n" +
                              "\"Bio\": \"" + bio + "\",\n" +
                              "\"Image\": \"" + image + "\"\n" +
                              "}";
                    break;

                case 5: // Buy More Coins
                    reqType = "POST";
                    path += "transactions/coins";
                    string coins = "0";

                    Console.WriteLine("Please Enter the Amount of coins you want to Buy");

                    tmp = "-1";
                    do {
                        Console.Write("Amount of Coins between 1 and 200 < ");
                        tmp = Console.ReadLine();
                        if (tmp != "-1" && tmp != "" && tmp != "0") coins = tmp;
                    } while (tmp == "" || Int32.Parse(tmp)<= 0 || Int32.Parse(tmp) > 200 );

                    message = "{\n" +
                              "\"BuyAmount\": \"" + coins + "\"\n" +
                              "}";
                    break;
                
                case 6: // Buy Card  Packs
//To-DO
                    break;

                case 7: // Show All Owned Cards
                    //DONE
                    reqType = "GET";
                    path += "cards";
                    break;

                case 8: // Get User Card Deck
                    //DONE
                    reqType = "GET";
                    path += "decks";
                    break;

                case 9: // Put Owned Card in Deck
                    //DONE
                    reqType = "PUT";
                    path += "decks";
                    tmp = "0";
                    int intTmp = 0;

                    Console.WriteLine("Please enter the ID of the Card you want to add");
                    do
                    {
                        tmp = Console.ReadLine();
                    } while (tmp == "" || tmp == "0" || !Int32.TryParse(tmp,out intTmp));

                    message = "{\n" +
                              "\"CardId\": \"" + tmp + "\"\n" +
                              "}";
                    break;

                case 10: // Show ScoreBoard
                    //DONE
                    reqType = "GET";
                    path += "score";
                    break;

                case 11: // Open Trading Handler
//TO-DO
                    break;
                case 12: // Battle Handler
//TO-DO
                    break;
            }

            //SEND MESSAGE TO SERVER
            string answerString = "";
            if (message.Length == 0)
            {
                answerString =
                        reqType + " " + path + " " + "HTTP/1.1\n" +
                        "Host: " + uri + "\n" +
                        "Authentication: " + authentication + "\n" +
                        "Content-Type: application/json; charset=utf-8\n";
            }
            else
            {
                answerString =
                        reqType + " " + path + " " + "HTTP/1.1\n" +
                        "Host: " + uri + "\n" +
                        "Authentication: " + authentication + "\n" +
                        "Content-Length:" + message.Length + " \n" +
                        "Content-Type: application/json; charset=utf-8\n" +
                        "\n" + message;
            }

            Byte[] reply = Encoding.ASCII.GetBytes(answerString);
            try
            {
                stream.Write(reply, 0, reply.Length);
                stream.Flush();
                Console.WriteLine("Sent: \n {0}", answerString);
            }
            catch (System.IO.IOException e)
            {
                Console.Write("Server ended connection!", e);
            }
        }

    }
}
