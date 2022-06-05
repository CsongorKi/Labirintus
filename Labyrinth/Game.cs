using System;
using System.Collections.Generic;
using System.IO;
using static System.Console;

namespace Labyrinth
{
    class Game
    {
        private World MyWorld;
        private Player CurrentPlayer;
        int palyaValaszt = 0;
        int playerStartX = 0;
        int playerStartY = 0;


        string[,] grid = null; 

        public void Start()
        {
            Title = "Welcome to the Maze!";
            CursorVisible = false;
            RunGameLoop();
        }

        private void DisplayIntro()
        {
            WriteLine("Welcome to the maze!");
            WriteLine("\nInstructions");
            WriteLine("> Use the arrow keys to move");
            Write("> Try to reach the goal, which looks like this: ");
            ForegroundColor = ConsoleColor.Green;
            WriteLine("X");
            ResetColor();
            Write("> Choose a map(1/2/3): ");
            palyaValaszt = Convert.ToInt32(Console.ReadLine());
            LoadMap(palyaValaszt);
            MyWorld = new World(grid);
            CurrentPlayer = new Player(playerStartY, playerStartX);
            WriteLine("> Press any key to start");
            ReadKey(true);
        }

        private void DisplayOutro()
        {
            Clear();
            WriteLine("You escaped!");
            WriteLine("Thanks for playing.");
            WriteLine("> Press any key to exit...");
            ReadKey(true);
        }

        private void DrawFrame()
        {
            Clear();
            MyWorld.Draw();
            CurrentPlayer.Draw();
        }

        private void LoadMap(int mapId)
        {
            char[] currentLine = null;
            string[] map = File.ReadAllLines($"Map{mapId}.txt");
            string[,] finalMap = new string[map.Length,map[0].ToCharArray().Length];
            for (int i = 0; i < map.Length; i++)
            {
                currentLine = map[i].ToCharArray();
                for (int j = 0; j < currentLine.Length; j++)
                {
                    if (currentLine[j].ToString() == "O")
                    {
                        playerStartX = i;
                        playerStartY = j;
                        finalMap[i, j] = " ";
                    }
                    else
                        finalMap[i, j] = currentLine[j].ToString();
                }
            }
            grid = finalMap;
        }

        private void HandlePlayerinput()
        {
            ConsoleKeyInfo keyInfo = ReadKey(true);
            ConsoleKey key = keyInfo.Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (MyWorld.IsPositionWalkable(CurrentPlayer.X, CurrentPlayer.Y - 1))
                    {
                        CurrentPlayer.Y -= 1;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (MyWorld.IsPositionWalkable(CurrentPlayer.X, CurrentPlayer.Y + 1))
                    {
                        CurrentPlayer.Y += 1;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (MyWorld.IsPositionWalkable(CurrentPlayer.X - 1, CurrentPlayer.Y))
                    {
                        CurrentPlayer.X -= 1;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (MyWorld.IsPositionWalkable(CurrentPlayer.X + 1 , CurrentPlayer.Y))
                    {
                        CurrentPlayer.X += 1;
                    }
                    break;
                default:
                    break;
            }
        }

        public void RunGameLoop()
        {
            DisplayIntro();
            while (true)
            {
                // Draw everything.
                DrawFrame();

                // Check for player input from the keyboard and move the player.
                HandlePlayerinput();

                // Check if the player has reached the exit and end the game if so.
                string elementAtPlayerPos = MyWorld.GetElementAt(CurrentPlayer.X, CurrentPlayer.Y);
                if (elementAtPlayerPos == "X")
                {
                    break;
                }
                
                // TODO!

                // Give the Console a chance to render.
                System.Threading.Thread.Sleep(20);
            }
            DisplayOutro();
        }
    }
}
