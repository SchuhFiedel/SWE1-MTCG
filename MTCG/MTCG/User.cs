using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG
{
    public class User
    {
        public string username = "";
        private string password = "";
        public string bio = "";
        public string image = "";

        public int user_id = -1;
        public int coins = -1;
        public int elo = -1;
        public int num_games = -1;

        private string sessionToken = "";

        public User()
        {}

        public User(string usr, string pwd, string info, string img, int id, int coins, int elo, int games)
        {
            SetAll(usr, pwd, info, img, id, coins, elo, games);
        }

        public void SetAll(string usr, string pwd, string info, string img, int id, int coins, int elo, int games)
        {
            this.username = usr;
            this.password = pwd;
            this.bio = info;
            this.image = img;

            this.user_id = id;
            this.coins = coins;
            this.elo = elo;
            this.num_games = games;
        }


        //this function can be changed in the future, but it should not make a big difference
        public string SetSessionToken(string username, string pwd) {
            DateTime dateToDisplay = DateTime.Now;

            string sessionToken = username + pwd + "-mtcgToken"+ dateToDisplay.Second.ToString();
            this.sessionToken = sessionToken;
            return sessionToken;
        }
    }
}
