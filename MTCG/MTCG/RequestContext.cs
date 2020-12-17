using System;
using System.IO;
using System.Collections.Generic;



namespace MTCG.Server
{
    public class RequestContext
    {
        public string Context(string data, List<string> messages)
        {
            StringReader reader = new StringReader(data);
            string[] messageLines = data.Split('\n');

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
                            allMsg += counter + ": " + x +  "\n";
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
                            int rowcounter = 0;
                            foreach (string x in messageLines)
                            {
                                rowcounter += 1;
                                if (x == "\r" || x == "\n" || x == "")
                                {
                                    break;
                                }
                            }

                            string addMessage = "";
                            for (; rowcounter < messageLines.Length; rowcounter++)
                            {
                                addMessage += messageLines[rowcounter];
                                if ((messageLines.Length - 1 - rowcounter) > 0)
                                {
                                    addMessage += "\n";
                                }
                            }

                            messages[Int32.Parse(tokens[2])] = addMessage;
                            response = "Changed " + tokens[2] + ": " + addMessage;
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
                        int rowcounter = 0;
                        foreach (string x in messageLines) {
                            rowcounter += 1;
                            if (x == "\r" ||x == "\n" || x== "")
                            {
                                break;
                            }
                        }

                        string addMessage = "";
                        for (;rowcounter < messageLines.Length; rowcounter++)
                        {
                            addMessage += messageLines[rowcounter];
                            if ((messageLines.Length-1 - rowcounter) > 0)
                            {
                                addMessage += "\n";
                            }
                        }
                        messages.Add(addMessage);
                        response += "Added: " + addMessage + " To ID: " + (messages.Count - 1);

                    }
                    else{
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
