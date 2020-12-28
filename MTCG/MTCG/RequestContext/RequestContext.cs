using System;
using System.IO;
using System.Collections.Generic;
using MTCG.Util;
using MTCG;



namespace MTCG.Server
{
    public class RequestContext
    {
        PostgreSqlClass DB = new PostgreSqlClass();
        User user = new User();
        string authentication = "";

        public Tuple<string,string> Context(string data, List<string> messages)
        {
            //processing data
            string[] messageLines = data.Split('\n');
            string[] tokens = messageLines[0].Split(' '); // first row
            string request = tokens[0]; //first row first token (GET/POST/PUT/DEL)
            string path = tokens[1]; //first row second token
            tokens = path.Split('/'); //split path in messages and id

            Console.WriteLine(data);

            Tuple<string, string> response = Tuple.Create("CONNECTION CLOSED!","EXIT");

            switch (tokens[1])
            {
                case "users":
                    //register = POST users; show user Info = GET users/username; change info = PUT users/username  
                    switch (request)
                    {
                        case "POST":
                            response = UserRegistration(messageLines);
                        break;
                        case "GET":
                            //response = GetUserInfos(messageLines);
                           response = Tuple.Create(authentication,"EXIT");
                        break;
                        case "PUT":
                            //reponse = Change Success / No Success
                        break;
                    }
                    break;
                case "sessions":
                    //login = POST session,
                    switch (request)
                    {
                        case "POST":
                            response = LoginUser(messageLines);
                            break;
                    }

                    break;
                case "packages":
                    //create = POST packages, 
                    break;
                case "transactions":
                    //buy = POST transactions/packages
                    break;
                case "cards":
                    //show all of user = GET cards
                    break;
                case "deck":
                    //show deck of user = GET deck; config deck = PUT deck
                    break;
                case "stats":
                    //show user stats = GET stats;
                    break;
                case "score":
                    //show scoreboard = GET score
                    break;
                case "tradings":
                    //show all tradings = GET tradings; trade = POST tradings, delete deal = DELETE tradings/tradeID
                    break;
                case "battle":
                    //go to matchmaking and fight = POST battles
                    break;
                default:
                    
                    break;
            }

            /*
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
                    } else{
                        response = "ERROR - Wrong number of arguments!";
                    }
                    break;

                case "PUT":
                    if (tokens[1] == "messages" && tokens.Length == 3){
                        if (messages.Count > Int32.Parse(tokens[2])){
                            //change message
                            int rowcounter = SearchDataRow(messageLines);
                            string addMessage = AddMessage(rowcounter,messageLines);

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
                        int rowcounter = SearchDataRow(messageLines);
                        string addMessage = AddMessage(rowcounter, messageLines);

                        messages.Add(addMessage);
                        response += "Added: " + addMessage + " To ID: " + (messages.Count - 1);

                    }else{
                        response = "ERROR - Wrong number of arguments!";
                    }
                    break;

                default:
                    response = "ERROR";
                    break;
            }*/
            return response;
        }

        //search for row in which the HTTP Payload is
        private int SearchDataRow(string[] messageLines)
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

        //add message from start of message until end 
        private string AddMessage(int rowcounter, string[] messageLines)
        {
            string addMessage = "";
            for (; rowcounter < messageLines.Length; rowcounter++)
            {
                addMessage += messageLines[rowcounter];
                if ((messageLines.Length - 1 - rowcounter) > 0) //if in the specified part of the message
                {
                    addMessage += "\n";
                }
            }
            return addMessage;
        }

        private Tuple<string, string> UserRegistration(string[] messageLines)
        {
            Tuple<string, string> response;
            Console.WriteLine("User Registration");
            int rowcounter = SearchDataRow(messageLines);
            string info = AddMessage(rowcounter, messageLines);

            info = info.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace("{", "").Replace("}", "").Replace("\"", "").Replace(" ", "");
            string[] attr = info.Split(',');

            string username = "";
            string password = "";
            for (int i = 0; i < attr.Length; i++)
            {
                string[] rowinfo = attr[i].Split(':');
                if (rowinfo[0] == "Username") { username = rowinfo[1]; }
                if (rowinfo[0] == "Password") { password = rowinfo[1]; }
            }

            try
            {
                DB.RegUser(username, password);
                response = Tuple.Create(
                            "{\n" +
                            "\"Registration\": \"Successful\"\n"+
                            "}","EXIT");
            }
            catch (Npgsql.PostgresException e)
            {
                response = Tuple.Create("{\n" +
                            "\"Registration\": \"Unsuccessful\",\n" +
                            "\"DB-Exception\": \"" + e.Message + "\"\n" +
                            "}","EXIT");
            }

            return response;
        }

        private Tuple<string, string> LoginUser(string[] messageLines)
        {
            Tuple<string, string> response;
            Console.WriteLine("User Login");
            int rowcounter = SearchDataRow(messageLines);
            string info = AddMessage(rowcounter, messageLines);

            //process string in json format
            info = info.Replace("\n", "").Replace("\r", "").Replace("\t", "")
                       .Replace("{", "").Replace("}", "").Replace("\"", "").Replace(" ", "");

            string[] attr = info.Split(',');

            string username = "";
            string password = "";
            for (int i = 0; i < attr.Length; i++)
            {
                string[] rowinfo = attr[i].Split(':');
                if (rowinfo[0] == "Username") { username = rowinfo[1]; }
                if (rowinfo[0] == "Password") { password = rowinfo[1]; }
            }

            try
            {
                //get user info from DB
                user = DB.GetUser(username, password);
                string newToken = user.SetSessionToken(username, password); //make new token
                System.Threading.Thread.Sleep(1000);
                
                if(newToken!=DB.GetToken(user.user_id))
                {
                    System.Threading.Thread.Sleep(1000);
                    DB.Insert("INSERT INTO validtokens (token,user_id) VALUES('" + newToken + "'," + user.user_id + ")");
                }
                response = Tuple.Create("{\n" +
                            "\"Login\": \"Success\",\n" +
                            "\"Token\": \"" + newToken + "\"" +
                            "}","ALIVE");
                authentication = newToken;
            }
            catch (ArgumentException e)
            {
                //catch exception -> wrong arguments
                response = Tuple.Create("{\n" +
                            "\"Login\": \"Unsuccessful\",\n" +
                            "\"DB-Exception\": \"" + e.Message + "\"\n" +
                            "}","EXIT");
            }
            catch (Npgsql.PostgresException e)
            {
                //catch DB exception
                response = Tuple.Create("{\n" +
                            "\"Login\": \"Unsuccessful\",\n" +
                            "\"DB-Exception\": \"" + e.Message + "\"\n" +
                            "}","EXIT");
            }
            return response;
        }
    }

    
}
