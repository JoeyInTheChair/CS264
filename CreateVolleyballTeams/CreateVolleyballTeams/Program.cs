using System;
using static System.Console;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic;
using System.Reflection;

namespace CreateVolleyballTeams
{
    class Program
    {
        public static List<string> team1 = new List<string>();
        public static List<string> team2 = new List<string>();
        public static List<string> team3 = new List<string>();
        public static List<string> team4 = new List<string>();
        public static List<string> team5 = new List<string>();
        public static List<string> team6 = new List<string>();
        public static List<Player> setterList = new List<Player>();
        public static List<Player> hitterList = new List<Player>();
        public static List<Player> middleList = new List<Player>();
        public static List<Player> mainTeam = new List<Player>();
        public static List<Player> beginners = new List<Player>();
        public static int i = 1;
        static void Main(string[] args)
        {
            createPlayerList();
            storePositions(mainTeam);
            setSetter(setterList);
            setMiddle(middleList);
            setHitter(hitterList);
            setterList.Clear();
            middleList.Clear();
            hitterList.Clear();
            storePositions(beginners);
            setSetter(setterList);
            setMiddle(middleList);
            setHitter(hitterList);
            printTeams(team1);
            WriteLine("********************************");
            printTeams(team2);
            WriteLine("********************************");
            printTeams(team3);
            WriteLine("********************************");
            printTeams(team4);
            WriteLine("********************************");
            printTeams(team5);
            WriteLine("********************************");
            printTeams(team6);
        }
        public static void setMiddle(List<Player> pos)
        {
            var rand = new Random();
            var randMiddle = middleList.OrderBy(item => rand.Next());
            foreach (var middle in randMiddle)
            {
                string player = middle.getName() + " " + middle.getPosition() + ": " + middle.getRating();
                switch (i % 6)
                {
                    case 1:
                        team1.Add(player);
                        break;
                    case 2:
                        team2.Add(player);
                        break;
                    case 3:
                        team3.Add(player);
                        break;
                    case 4:
                        team4.Add(player);
                        break;
                    case 5:
                        team5.Add(player);
                        break;
                    default:
                        team6.Add(player);
                        break;
                }
                i++;
            }
        }
        public static void setHitter(List<Player> pos)
        {
            var rand = new Random();
            var randHitter = hitterList.OrderBy(item => rand.Next());
            foreach (var hitter in randHitter)
            {
                string player = hitter.getName() + " " + hitter.getPosition() + ": " + hitter.getRating();
                switch (i % 6)
                {
                    case 1:
                        team1.Add(player);
                        break;
                    case 2:
                        team2.Add(player);
                        break;
                    case 3:
                        team3.Add(player);
                        break;
                    case 4:
                        team4.Add(player);
                        break;
                    case 5:
                        team5.Add(player);
                        break;
                    default:
                        team6.Add(player);
                        break;
                }
                i++;
            }
        }
        public static void setSetter(List<Player> pos)
        {
            var rand = new Random();
            var randSetter = setterList.OrderBy(item => rand.Next());
            foreach (var setter in randSetter)
            {
                string player = setter.getName() + " " + setter.getPosition() + ": " + setter.getRating();
                switch (i % 6)
                {
                    case 1:
                        team1.Add(player);
                        break;
                    case 2:
                        team2.Add(player);
                        break;
                    case 3:
                        team3.Add(player);
                        break;
                    case 4:
                        team4.Add(player);
                        break;
                    case 5:
                        team5.Add(player);
                        break;
                    default:
                        team6.Add(player);
                        break;
                }
                i++;
            }
        }
        public static void storePositions(List<Player>pos)
        {
            foreach(Player player in pos)
            {
                switch (player.getPosition())
                {
                    case "setter":
                        setterList.Add(player);
                        break;
                    case "hitter":
                        hitterList.Add(player);
                        break;
                    default:
                        middleList.Add(player);
                        break;
                }
            }
        }
        public static void printTeams(List <string> team)
        {
            foreach(string player in team) 
            {
                WriteLine(player);
            }
        }
        public static void createPlayerList()
        {
            String line;
            try
            {
                StreamReader sr = new StreamReader("C:\\Users\\josep\\Desktop\\quiz4\\x64\\CreateVolleyballTeams\\CreateVolleyballTeams\\playerList.txt");
                line = sr.ReadLine();
                while(line != null)
                {
                    String[] temp = line.Split(' ');
                    Player player = new Player();
                    player.player(temp[0], temp[1], temp[2]);
                    switch(temp[2])
                    {
                        case "1":
                            mainTeam.Add(player);
                            break;
                        default:
                            beginners.Add(player);
                            break;
                    }
                    line = sr.ReadLine();
                }
                sr.Close();
                Console.ReadLine();
            }
            catch(Exception e)
            {
                WriteLine("Exception: " + e.Message);
            }
        }
    }
}