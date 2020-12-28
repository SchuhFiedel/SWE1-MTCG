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

        public string SetSessionToken(string username, string pwd) {
            string sessionToken = username + pwd + "-mtcgToken";
            this.sessionToken = sessionToken;
            return sessionToken;
        }
    }
}
