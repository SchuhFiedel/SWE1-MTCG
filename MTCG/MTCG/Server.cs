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
        MatchmakingListWrapper userIDsForMatchmaking = new MatchmakingListWrapper(); // made Wrapper cclasses to be able to reference 
        List<int> loggedInUserIDs = new List<int>();
        FightingTupleWrapper fighting = new FightingTupleWrapper();
        BattleHandler battleHandlerObj = new BattleHandler();
        Mutex mut = new Mutex();
               

        public Server(string ip, int port)
        {
            IPAddress localAddr = IPAddress.Parse(ip);
            server = new TcpListener(localAddr, port);
            StartMatchMaker();
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
            Tuple<string, string> response = new Tuple<string, string>("", "");
            //run thread with connection to single client until client ends connection or Error happens
            bool stop = true;
            while (stop)
            {
                try
                {
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = Encoding.ASCII.GetString(bytes, 0, i);

                        //send StreamData to the Request Kontext, // Add userId to matchmaking, //return user connected to instance
                        //mut.WaitOne();
                        int instanceUser;
                        int userIDMatchMake;
                        response = req.Context(data, out userIDMatchMake, out instanceUser); // Response = Tuple (Response, ConnStatus)

                        if (instanceUser > 0) // if user_id is not 0 -> is logged in
                        {
                            if((fighting.fighting.Item1 == instanceUser || fighting.fighting.Item2 == instanceUser) && response.Item2 == "MATCH") // if user_id is fighting
                            {
                                if (!fighting.fighting.Item3) //if fight is over
                                {
                                    if(fighting.fighting.Item4 == true)
                                    {
                                        response = Tuple.Create<string, string>("{\n" +
                                                                               "\"Battle\": \"Over\",\n" +
                                                                               "\"Draw\": \"True!\"\n" +
                                                                               "}", "ALIVE");
                                    }
                                    else if(fighting.fighting.Item1 == instanceUser) // if user_id is winner
                                    {
                                        response = Tuple.Create<string,string>("{\n" +
                                                                               "\"Battle\": \"Over\",\n" +
                                                                               "\"Winner\": \"You Won!\",\n" +
                                                                               "\"Loser\": \"Oponent Lost!\"\n" +
                                                                               "}","ALIVE") ;
                                    }
                                    else // if user_id is not winner
                                    {
                                        response = Tuple.Create<string, string>("{\n" +
                                                                               "\"Battle\": \"Over\",\n" +
                                                                               "\"Winner\": \"You Won!\",\n" +
                                                                               "\"Loser\": \"Oponent Lost!\"\n" +
                                                                               "}", "ALIVE");
                                    }
                                    
                                }
                                else //if fight is not over
                                {
                                    Thread.Sleep(500);
                                    response = Tuple.Create<string, string>("{\n" +
                                                                            "\"Battle\": \"Fighting\"\n" +
                                                                            "}", "ALIVE");
                                }
                            }

                            if(!loggedInUserIDs.Contains(instanceUser)) loggedInUserIDs.Add(instanceUser); // if user not logged in log in user
                            if (response.Item2 == "MATCH" && !userIDsForMatchmaking.userIDsForMatchmaking.Contains(instanceUser) && userIDMatchMake > 0 && instanceUser == userIDMatchMake)
                            {
                                userIDsForMatchmaking.userIDsForMatchmaking.Add(userIDMatchMake);
                            }
                            else if(response.Item2 == "MATCH" && userIDsForMatchmaking.userIDsForMatchmaking.Contains(instanceUser) && userIDMatchMake == instanceUser)
                            {
                                Thread.Sleep(500);
                                response = Tuple.Create<string, string>("{\n" +
                                                                       "\"Matchmaking\": \"Waiting\"\n" +
                                                                       "}", "ALIVE");
                            }
                            else if (response.Item2 == "EXIT")
                            {
                                loggedInUserIDs.Remove(instanceUser);
                                userIDsForMatchmaking.userIDsForMatchmaking.Remove(instanceUser);
                                stop = false;
                            }
                        }
                        //mut.ReleaseMutex();
                        Thread.Sleep(500);
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

        void StartMatchMaker()
        {
            try
            {
                    Thread t = new Thread(StartBattleHandler);
                    t.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Matchmaking Exception: {0}", e);
                server.Stop();
            }
            
        }

        void StartBattleHandler()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if(userIDsForMatchmaking.userIDsForMatchmaking.Count >= 2)
                {
                    battleHandlerObj.PrepareBattle(ref fighting, ref userIDsForMatchmaking);
                }
                Thread.Sleep(10000);
                mut.WaitOne();
                fighting.fighting = Tuple.Create(0, 0, false, false);
                mut.ReleaseMutex();
            }
        }
    }
}

