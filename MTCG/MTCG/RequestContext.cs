using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;


namespace MTCG.Server
{
    class RequestContext
    {
        public string Context(string data, List<string> messages)
        {
            StringReader reader = new StringReader(data);
            string[] messageLines = data.Split('\n');

            /*
            foreach (string x in messageLines)
            {
                Console.WriteLine(x);
            }
            Console.WriteLine("\n\n\r\r");
            */

            string[] tokens = messageLines[0].Split(' ');

            string request = tokens[0];
            string path = tokens[1];

            tokens = path.Split('/');
            string response = "";

            switch (request){
                case "GET":
                    if (tokens[1] == "messages" && tokens.Length == 3){
                        //send specific message
                        if (messages.Count > Int32.Parse(tokens[2])){
                            response = "SPECIFIC MESSAGE on: " + tokens[2] + ": \n" + messages[Int32.Parse(tokens[2])];
                        }else{
                            response = "Print not possible - Message ID does not exist!";
                        }
                    }else if (tokens[1] == "messages" && tokens.Length <= 2){
                        //send all message
                        string allMsg = "";
                        int counter = 0;
                        foreach (string x in messages){
                            allMsg += counter + "\n";
                            counter++;
                        }
                        response = "ALL MESSAGES:\n " + allMsg;
                    }else{
                        response = "ERROR - Wrong number of arguments!";
                    }
                    break;
                case "PUT":
                    if (tokens[1] == "messages" && tokens.Length == 3){
                        if (messages.Count > Int32.Parse(tokens[2])){
                            //change message
                            messages[Int32.Parse(tokens[2])] = messageLines[messageLines.Length-1];
                            response = "Changed " + tokens[2] + " + " + messageLines[messageLines.Length - 1];
                        }else{
                            response = "Change not possible - Message ID does not exist!";
                        }
                    }else{
                        response = "ERROR - Wrong number of arguments!";
                    }
                    break;
                case "DELETE":
                    if (tokens[1] == "messages" && tokens.Length == 3){
                        if (messages.Count > Int32.Parse(tokens[2])){
                            //delete specific message
                            messages.RemoveAt(Int32.Parse(tokens[2]));
                            response = "Deleted " + tokens[2];
                        }else{
                            response = "Change not possible - Message ID does not exist!";
                        }
                    }else{
                        response = "ERROR - Wrong number of arguments!";
                    }
                    break;
                case "POST":
                    if (tokens[1] == "messages" && tokens.Length <= 2){
                        //add message from Payload
                        messages.Add(messageLines[messageLines.Length - 1]);
                        response = "Added: " + messageLines[messageLines.Length - 1] + " To ID: " + (messages.Count-1);
                        
                    }else{
                        response = "ERROR - Wrong number of arguments!";
                    }
                    break;
                default:
                    response = "ERROR";
                    break;
            }
            return response;
        }

    }

    
}
