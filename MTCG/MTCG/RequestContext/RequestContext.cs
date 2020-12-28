using System;
using System.IO;
using System.Collections.Generic;
using MTCG.Util;
using MTCG;
using MTCG.Cards;



namespace MTCG.Server
{
    public class RequestContext
    {
        PostgreSqlClass DB = new PostgreSqlClass();
        User user = new User();
        List<Card> userCardStack = new List<Card>();
        string authentication = "";

        public Tuple<string, string> Context(string data, List<string> messages)
        {
            //processing data
            string[] messageLines = data.Split('\n');
            string[] tokens = messageLines[0].Split(' '); // first row
            string request = tokens[0]; //first row first token (GET/POST/PUT/DEL)
            string path = tokens[1]; //first row second token
            tokens = path.Split('/'); //split path in messages and id
            int rowcounter = SearchDataRow(messageLines); // find row where Header ends
            Tuple<string[], string> allData = SplitData(rowcounter, messageLines); // split messageLines in HeaderArray and Payload string
            Dictionary<string, string> headers = GetHeaders(allData.Item1); //Split Header Array into Dictinary with Attributes and Values

            Console.WriteLine(data);

            Tuple<string, string> response = Tuple.Create("CONNECTION CLOSED!", "EXIT");

            switch (tokens[1])
            {
                case "users":
                    //DONE
                    //register = POST users; show user Info = GET users/username; change info = PUT users/username  
                    switch (request)
                    {
                        case "POST":
                            //DONE
                            //Register new user 
                            response = UserRegistration(allData.Item2);
                            break;
                        case "GET":
                            //DONE
                            //Send back all user information 
                            if (CheckAuthenticity(allData.Item2, headers))
                            {
                                response = Tuple.Create(GetUserInfo(user), "ALIVE");
                            }else
                            {
                                response = Tuple.Create("{\n" +
                                                        "\"Query\": \"Unsuccessful\"\n" +
                                                        "\"Error\": \"WrongToken\"\n" +
                                                        "}", "EXIT");
                            }
                            break;
                        case "PUT":
                            //DONE
                            //reponse = Change Success / No Success 
                            if (CheckAuthenticity(allData.Item2, headers))
                            {
                                if (ChangeUserInfo(user,allData.Item2,tokens[2]))
                                {
                                    response = Tuple.Create("{\n" +
                                                            "\"Change\": \"Success\"\n" +
                                                            "}", "ALIVE");
                                }
                                else
                                {
                                    response = Tuple.Create("{\n" +
                                                            "\"Change\": \"Unsuccessful\"\n" +
                                                            "}", "ALIVE");
                                }
                            }
                            else
                            {
                                response = Tuple.Create("{\n" +
                                                        "\"Change\": \"Unsuccessful\",\n" +
                                                        "\"Error\": \"WrongToken\"\n"+
                                                        "}", "EXIT");
                            }
                            break;
                    }
                    break;

                case "sessions":
                    //TO-DO
                    //login = POST sessions, logour = DELETE sessions 
                    switch (request)
                    {
                        //DONE
                        //Login = POST sessions -> sets authentication token in RequestContext and DB
                        case "POST":
                            response = LoginUser(allData.Item2);
                            break;
                        //TO-DO
                        //Logout = DELETE sessions -> delete token in DB, end connection to client, close client, close Handler Thread
                        case "DELETE":
                            break;
                    }

                    break;
                case "packages":
                    //TO-DO
                    //create = POST packages, ONLY IF ADMIN
                    break;
                case "transactions":
                    //TO-DO
                    //buy = POST transactions/packages
                    break;
                case "cards":
                    //TO-DO
                    //show all of user = GET cards
                    switch (request)
                    {
                        case "GET":

                            userCardStack = DB.GetAllUserCards(user);
                            string serialCards = SerializeCards(userCardStack);
                            response = Tuple.Create(serialCards, "ALIVE");
                            break;
                    }
                    break;
                case "deck":
                    //TO-DO
                    //show deck of user = GET deck; config deck = PUT deck
                    switch (request)
                    {
                        case "GET":
                            //response = GetUserDeckCards();
                            break;
                        case "PUT":
                            //response = AddCardsToUserDeck();
                            break;
                    }
                    break;
                case "stats":
                    //TO-DO
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

        //Make Json format strings more readable
        private string RemoveUnnecessaryChars(string info)
        {
            info = info.Replace("\n", "").Replace("\r", "").Replace("\t", "")
                        .Replace("{", "").Replace("}", "").Replace(" \"", "").Replace("\"", "");
            return info;
        }

        //Check if authentication token is the same as saved for this login
        private bool CheckAuthenticity(string payload, Dictionary<string, string> headers)
        {
            string info;
            if (!headers.TryGetValue("Authentication", out info))
                return false;

            if (info == authentication)
                return true;
            else
                return false;
        }

        //Split data into Headers and Payload
        private Tuple<string[], string> SplitData(int rowcounter, string[] messageLines)
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
        private Dictionary<string, string> GetHeaders(string[] data)
        {
            Dictionary<string, string> Headers = new Dictionary<string, string>();

            for (int i = 1; i < data.Length - 1; i++)
            {
                string[] tmp = data[i].Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "").Split(":");
                Headers.Add(tmp[0], tmp[1]);
            }
            return Headers;
        }

        //register user in the DB
        private Tuple<string, string> UserRegistration(string info)
        {
            Tuple<string, string> response;
            //Console.WriteLine("User Registration");

            info = RemoveUnnecessaryChars(info);
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
                            "\"Registration\": \"Successful\"\n" +
                            "}", "EXIT");
            }
            catch (Npgsql.PostgresException e)
            {
                response = Tuple.Create("{\n" +
                            "\"Registration\": \"Unsuccessful\",\n" +
                            "\"DB-Exception\": \"" + e.Message + "\"\n" +
                            "}", "EXIT");
            }

            return response;
        }

        //login user -> save their authentication token and user data
        private Tuple<string, string> LoginUser(string info)
        {
            Tuple<string, string> response;
            //Console.WriteLine("User Login");

            //process string in json format
            info = RemoveUnnecessaryChars(info);

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
                user = DB.GetUser(username);
                string newToken = user.SetSessionToken(username, password); //make new token

                if (DB.GetToken(user.user_id).Length <= 0)
                {
                    DB.Insert("INSERT INTO validtokens (token,user_id) VALUES('" + newToken + "'," + user.user_id + ")");
                }
                else
                {
                    DB.Insert("DELETE FROM validtokens where user_id = " + user.user_id);
                    DB.Insert("INSERT INTO validtokens (token,user_id) VALUES('" + newToken + "'," + user.user_id + ")");
                }
                response = Tuple.Create("{\n" +
                            "\"Login\": \"Success\",\n" +
                            "\"Token\": \"" + newToken + "\"" +
                            "}", "ALIVE");
                authentication = newToken;
            }
            catch (ArgumentException e)
            {
                //catch exception -> wrong arguments
                response = Tuple.Create("{\n" +
                            "\"Login\": \"Unsuccessful\",\n" +
                            "\"DB-Exception\": \"" + e.Message + "\"\n" +
                            "}", "EXIT");
            }
            catch (Npgsql.PostgresException e)
            {
                //catch DB exception
                response = Tuple.Create("{\n" +
                            "\"Login\": \"Unsuccessful\",\n" +
                            "\"DB-Exception\": \"" + e.Message + "\"\n" +
                            "}", "EXIT");
            }
            return response;
        }
       
        //get all information on user
        private string GetUserInfo(User user)
        {
            string userInfo = "";
            List<string> userData = new List<string>
            {
                user.username,
                user.bio,
                user.image,
                user.coins.ToString(),
                user.elo.ToString(),
                user.num_games.ToString()
            };

            for (int i = 0; i < userData.Count; i++)
            {
                if (userData[i] == "")
                {
                    userData[i] = "NAN";
                }
            }

            userInfo = "{\n" +
                        "\"Query\": \"Success\"\n" +
                        "\"Username\": \"" + userData[0] + "\"\n" +
                        "\"Bio\": \"" + userData[1] + "\"\n" +
                        "\"Image\": \"" + userData[2] + "\"\n" +
                        "\"Coins\": \"" + userData[3] + "\"\n" +
                        "\"Elo\": \"" + userData[4] + "\"\n" +
                        "\"Number-of-games\": \"" + userData[5] + "\"\n" +
                        "}";

            return userInfo;
            
        }

        //Change Username, Bio and Image of User
        private bool ChangeUserInfo(User user, string info, string pathUser)
        {
            bool success = false;
            //process string in json format
            info = RemoveUnnecessaryChars(info);

            string[] attr = info.Split(','); // split info in attributes

            string newUsername = "";
            string newBio = "";
            string newImage = "";
            for (int i = 0; i < attr.Length; i++)
            {
                string[] rowinfo = attr[i].Split(':'); // split row into attr and values
                if (rowinfo[0] == "Username" ) { // to change the username, the path hast to be "users/currentUesername" and the Payload has to include the new username
                    if(user.username == pathUser){
                        newUsername = rowinfo[1];
                    }
                    else newUsername = user.username;
                }
                if (rowinfo[0] == "Bio") { newBio = rowinfo[1]; } // change Bio from payload
                if (rowinfo[0] == "Image") { newImage = rowinfo[1]; } // change image from payload
            }

            try
            {   //Update New data in db
                if (newUsername == user.username)
                {
                    DB.Insert("UPDATE usertable set bio=\'" + newBio + "\', image=\'" + newImage + "\' WHERE user_id = "+ user.user_id + ";"); 
                }
                else
                {
                    DB.Insert("UPDATE usertable set username = \'" + newUsername + "\', bio=\'" + newBio + "\', image=\'" + newImage + "\'WHERE user_id = " + user.user_id + ";");
                }
                //update new data in Local User Object
                this.user = DB.GetUser(newUsername);
                success = true;
            }
            catch (Npgsql.PostgresException)
            {
                success = false;
            }

            return success;
        }

        private string SerializeCards(List<Card> cardList)
        {
            string response = "{\n\"CardNum\":\""+cardList.Count+"\",\n";
            for(int i = 0; i<cardList.Count; i++)
            {
                response += "\"Card\":\n";
                response += "{\n";
                response += "\"CardName\": \""+ cardList[i].GetCardName() + "\",\n";
                response += "\"CardInfo\": \"" + cardList[i].GetCardInfo() + "\",\n";
                response += "\"CardType\": \"" + cardList[i].GetCardType() + "\",\n";
                response += "\"CardSpecial\": \"" + cardList[i].GetSpecial() + "\",\n";
                response += "\"CardHP\": \"" + cardList[i].GetHP() + "\",\n";
                response += "\"CardAP\": \"" + cardList[i].GetAP() + "\",\n";
                response += "\"CardDP\": \"" + cardList[i].GetDP() + "\",\n";
                response += "\"CardPiercing\": \"" + cardList[i].GetPiercing() + "\",\n";
                response += "}\n";
            }
            response += "}";
                                 
            return response;
        }
    }
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
