# SWE1-MTCG
Monster Trading Card Game and REST HTTP-based plain-text Webservices

[Hier geht's zur git repo(client/server Branch)](https://github.com/SchuhFiedel/SWE1-MTCG/tree/DevServerClient)

usertable:
Every Colum is set with "NOT NULL" Constraint
user_id (PK,Int,Serial) | username (Text,Unique) | password (text) | coins(Int) | elo(Int) | num_games(Int)

cards:
i_cid(PK,INT,Serial) | c_cardname(varchar20) | c_cardinfo(text) | b_cardtype(bool) | i_elementtype(Int) | i_specialtype(Int) | i_maxhp(Int) | i_maxdp(int) | b_piercing(bool)

user_cards:


cardpacks:


cardpacks_cards:


