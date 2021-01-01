# SWE1-MTCG by Maximiliano Zulli (BIF3C1 IF19B183)
Monster Trading Card Game and REST HTTP-based plain-text Webservices

[Hier geht's zur git repo(client/server Branch)](https://github.com/SchuhFiedel/SWE1-MTCG/tree/DevServerClient)
Meine Zeiterfassung für dieses Projekt befindet sich in folgender Datei: ***MTCG_Zeiterfassung_Zulli.pdf***

In dieser ReadMe Datei befinden sich alle Informationen zu diesem Projekt, die auch Teilweise genau beachtet werden müssen damit es ausgeführt werden kann. Als erstes werden die wesentlichsten Punkte genannt welche für die ausführung des Programmes relevant sind. Danach werden unter anderem das DB-Schema aufgezeigt, Designentscheidungen, Probleme und weitere Informationen über das Projekt generell.
Ich habe mich für dieses Projekt entschieden einen eigenen Integration Test inform zweier Demo Modes in meinem Client Programm zu schreiben.

## DataBase Set Up
Damit mein Server sich mit der Datenbank verbinden kann muss sie folgendermaßen konfiguriert sein:
- PostgreSQL DB lokal installiert
- Verbindungsdaten müssen so konfiguriert sein: ***"Host=localhost;Username=postgres;Password=postgres;Database=mtcg;Pooling=false"]*** -> falls die DB Login daten unterschiedlich sind müssen diese in der Datei ***SWE1-MTCG/MTCG/Util/PosgreSqlClass.cs - FUNCTION SetConnect()*** verändert werden damit sich das Programm verbinden kann.
- Die mit Daten Befüllte Datenbank Tables, Trigger und Sequences befinden sich in folgendem File: ***DB_export_Zulli.txt***.
- Das Datenbankscript sollte ausgeführt werden bevor das Server/Client Programm gestartet wird.

## Server/Client Setup
- Das Server Programm (MTCG) sollte immer vor den Clients gestartet werden.
- Wenn ein Client Programm gestartet wird, muss angegeben werden ob ein Demomodus oder der normale Client Modus ausgeführt werden soll.
- Demo Modus 1 beinhaltet alle funktionen die der Client bzw der Server hat
- Demo Modus 2 führt nur alles aus was relevant ist um einen Kampf zu starten
- Um den Demo-Modus richtig auszuführen muss in einem Client Demo1 ausgeführt werden und im zweiten Demo2
- Nachdem die Zwei Clients gekämpft haben werden beide Clients ausgelogged und beendet.

Falls nicht ein Demo Modus gestartet wird sondern der normale Client modus, muss der User Manuell alle funktionen des Clients ausführen, und kann auch einen kampf starten wenn er angemeldet ist und ein KartenDeck mit 4 Karten erstellt hat. Der User sollte sich immer vor beenden des Clients abmelden.

## Funktionen (funktionierend Y / nicht funktionierend N)
Functional Requirements
- Battle
-- play rounds Y
-- draw possible Y
-- battle Log N
-- specials are considered Y
- user management (Registration, Profile Page, Change profile, login, Logout) Y
- scoreboard Y
- card Trading N
- mandatory Unique Feature (4 More element Types for cards)

Nonfunctional Requirements
- Token Based Security Y
- Persistent DB Y
- Unit Tests N
- Integration Tests (Demo Mode Clients) Y

Protocol
- Design/Lessons Learned Y
- Unit Test Design N (keine Unit Tests)
- Time Spent On Project Y (beiliegendes EXCEL Sheet)
- Link to GitRepo Y

Additional Features
- Elo System
- Wins Losses
- Win Loss Rate
- Client with Menu
- Session Logout
- Buy Coins
- Card Descriptions
- Extra ElementTypes

## Design 
### Server
Grundsätzlich wir jede Funktion des Servers nur durch die richtige Kombination aus HTTP-Request, Pfad des Requests, Authentifizierungstoken und HTTP-Payload in JSON Format ausgeführt, kommt es hier zu einer übertragung die vom Server nicht erwünscht ist wird die Verbindung zum Client sofort gekappt.

Der Server startet zu allererst einen "BattleHandler"-Thread welcher darauf wartet, dass 2 oder mehr Spieler einen Kampf beginnen wollen (siehe Kapitel Battlehandler), und wartet auf eine Verbindung mit einem Client.

#### Authentifizierung
Das Design des Servers war von Anfang an darauf ausgelegt, dass sich ein User nach einer erfolgreichen Verbindung mit dem Server anmelden muss um weitere Funktionalitäten freizuschalten. Der server startet bei einem Verbindungsaufbau eines neuen Clients, einen eigenen Thread welcher von nun an für die Handhabung der Befehle des Clients verantworlich ist. Bei der Anmeldung wird der Username und das Passwort des Users an den Server(Thread) geschickt, der dann wiederum einen eindeutigen (für diesen User und Session) Authentifizierungstoken erstellt und an den User zurücksendet. Gleichzeitig speichert der ServerThread die UserDaten aus der Datenbank lokal, um sie später für weiter Funktionalitäten zu verwenden. 

Ab diesem Zeitpunkt erwartet der ServerThread bei jeder Anfrage des Clients auch dieses spezifische Authentifizierungstoken im Header des HTTP-Requests. Aus diesem Grund wird es nach Ersterhalt beim Client auch lokal gespeichert.

#### Request-Handling
Jeder Request der beim Server empfangen wird, wird an den RequestContext übergeben. Dieser Bearbeitet den empfangenen String und ruft je nach Request unterschiedliche Funktionen auf, welche dann bestimmte Antworten zurückgeben. Diese können entweder Fehler, Erfolge oder angesuchte Daten sein. Dieser Response String, und ein weiterer "Status"-String werden an die Hauptfunktion des Threads zurückübergeben welcher dann den Response string an den Client zurücksendet und/oder auf den "Status"-String reagiert und die Verbindung beendet, den Kampfmodus startet, oder nichts verändert und nur sendet.

Es gibt mehrere Funktionen im RequestContext-Objekt welche auschließlich den Empfangenen Request String für andere Funktionen aufbereiten, oder z.B. überprüfen ob ein Client einen Angemeldeten User hat oder nicht. 

Wenn der User Karten für sein Deck ausgewählt hat und einen "POST /battle" request absendet wird setzt sich eine Reihe von Dingen in Bewegung:

#### Battle-Modus
Zu Battle-Modus befindet sich das Matchmaking. Wenn der RequestContext erkennt, dass ein User Kämpfen will, sendet er diese information zurück an die darüberliegende "HandleDevice"-Funktion(Thread). Dieser reagiert auf diese information indem der User, in die MatchmakingListe hinzugefügt wird und der HandleThread in den Battle-Modus übergeht. Der Server sendet dem Client nun die Information, dass das Matchmaking begonnen hat.

Wenn der Client diese Nachricht bekommt, startet dieser seinen battle Modus, was zur Folge hat, dass dieser bis zum Ende des Kampfes, Nachrichten an den Server sendet um zu erfahren was der Kampf-Status ist.

Wenn nun 2 oder mehr Clients/User sich im Matchmaking bzw Battle-Modus befinden, führt der Battle-Handler das Battle aus:
Beide Clients werden aus der Matchmaking queue entfert und bekommen den status "im Kampf". Als erstes holt sich der Battle-Handler alle Informationen die er braucht aus der Datenbank (z.B. die Decks der Spieler). Nun wird die Kamp Logik ausgeführt welche bei einem Eindeutigen Spiel zu einem Gewinner und Verlierer oder zu einem Unentschieden führt. Den Spielern wird ein "gespieltes-Spiel", ein Gewinn oder Verlust und die jeweilige Veränderung der ELO-Punkte angerechnet. 

Nach ende des Kampfes bekommen Beide Spieler eine Nachricht darüber wer Gewonnen hat, oder ob es ein Unentschieden war. Wenn die Clients diese Nachricht empfangen beenden sie schließlich, genau wie der Server den Battle-Modus und zeigen dem User wieder das Menü.

#### Kampf Logik
Vor dem Spiel wird zufällig entschieden welcher Spieler beginnt.

- Jeder Spieler hat 4 Karten in seiner Hand (Deck) (am Anfang des Spiels)
- Jede Runde Spielt jeder Spieler (wenn dieser an der Reihe ist) die erste Karte aus seiner Hand auf das Spielfeld 
- Jede Runde greift jede Karte welche auf dem Spielfeld liegt eine zufällige gegnerische Karte an
- Monster Karten haben HealthPoints - wenn diese auf 0 sind wird die Karte aus dem Spiel entfernt
- Zauber Karten haben eine maximale Anzahl an Verwendungen - wenn eine Zauberkarte eine Monsterkarte angreift wird dies Zahl -1 gerechnet; wenn sie auf 0 ist wird die Karte aus dem Spiel entfernt
- Monster können keine Zauber angreifen, genausowenig wie Zauber Zauber angreifen können
- Wenn beide Spieler keine Karten mehr legen können, und alle Karten auf dem Spielfeld entfernt oder Zauberkarten sind endet das Spiel.
- Der Spieler mit mehr Monstern am Feld gewinnt

### Client
Der Client beinhaltet einerseitz die Demo Modi (1 und 2) und den normalen Client modus
#### Demo Modes
Wie ganz oben schon erwähnt, die zwei Demo Modi sind dazu da die Integration Tests zu ersetzen.
Der erste ist dazu da einen "Neuen"-User zu simulieren. Dieser Registriert sich mit neuen Nutzerdaten, Ändert seine Bio und sein Bild, kauft sich danach mehr Münzen und zwei Kartenpacks. Danach fügt er Karten in sein Deck hinzu und geht ins Matchmaking

Der zweite ist dazu da einen bestehenden User zu simulieren, welcher nur sein Kartendeck ändert und danach  in den battle Mode geht.

Beide User sehen sich das Scoreboard vor und nach dem Kampf an um zu sehen was sich verändert hat.

#### Client
Nachdem man den Client modus ausgewählt hat, kann man sich Registrieren, Anmelden oder Abmelden.
Wenn man sich mit einem neuen User registriert muss man sich danach Anmelden.
Wenn ein User sich angemeldet hat werden ihm Folgende Funktionen freigeschalten welche alle funktionieren:

- Alle User Informationen Anzeigen
- User Informationen Ändern
- Münzen Kaufen
- Karten Packages Kaufen
- Alle Karten Anzeigen Die Der User Besitzt
- Aktuelles Deck des Users Anzeigen
- Eine Karte Die Der User Besitzt Dem Deck Hinzufügen
- Scoreboard Anzeigen (alle Usernamen in Elo-Rank Reihenfolge mit Rang, Elo, Wins, Losses, und Win/Loss Rate)
- Kampf-Modus (Matchmaking)
- Logout Und Programm Beenden

### Karten
Karten haben Folgende Attribute: 
- CardID
- CardName
- CardInfo (Description)
- CardType (Spell, Monster)
- ElementType (None, Fire, Water, Ice, Earth, Air, Electro)
- SpecialType (None, Dragon, Goblin, Knight, Wizzard, Ork, Kraken, FireElf)
- HealthPoints
- AttackPoints
- DefensePoints
- Piercing (Yes, No)

### Datenbank "Schema"
##### usertable:
Every Colum is set with "NOT NULL" Constraint
user_id (PK,Int,Serial) | username (Text,Unique) | password (text) | coins(Int) | elo(Int) | num_games(Int) | bio(varchar(30)) | image(varchar(30)) | loss(int) | win(int)

##### cards:
i_cid(PK,INT,Serial) | c_cardname(varchar20) | c_cardinfo(text) | b_cardtype(bool) | i_elementtype(Int) | i_specialtype(Int) | i_maxhp(Int) | i_maxdp(int) | b_piercing(bool)

##### cardstacks:
stack_id(PK,INT,Serial) | user_id(int, fk) | card_id(int, fk)

##### carddecks:
user_id(PK, fk, int) | card_id(PK, fk, int)

##### packages:
pack_id(PK, int) | card_id(PK, fk, int)

##### validtokens:
token_id(PK,int) | token(text) | user_id(fk,int)
