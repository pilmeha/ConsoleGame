//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System;

namespace ConsoleGame
{
    class Program
    {
        
        private const int ScreenWidth = 100;
        private const int ScreenHeight = 50;

        float aspect = (float)(ScreenWidth / ScreenHeight);
        float pixelAspect = 11.0f / 24.0f;

        private const int MapWidth = 32;
        private const int MapHeight = 32;

        private const double Fov = Math.PI / 3;
        private const double Depth = 16;

        private static double _playerX = 3;
        private static double _playerY = 3;
        private static double _playerA = 0;


        private static string _map = "";

        private static readonly char[] Screen = new char[ScreenWidth * ScreenHeight];

        static void Main(string[] args)
        {
            Console.SetWindowSize(ScreenWidth, ScreenHeight);
            Console.SetBufferSize(ScreenWidth, ScreenHeight);
            Console.CursorVisible = false;

            _map += "################################";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "################################";

            //char c = ' ';

            while (true)
            {
                //if (Console.KeyAvailable)
                //{
                //    c = Console.ReadKey().KeyChar;
                //}

                //Array.Fill(Screen, c);
                for (int x = 0; x < ScreenWidth; x++)
                {
                    double rayAngle = _playerA + Fov / 2 - x * Fov / ScreenWidth;

                    double rayX = Math.Sin(rayAngle);
                    double rayY = Math.Cos(rayAngle);

                    double distanceToWall = 0;
                    bool hitWall = false;

                    while (!hitWall && distanceToWall < Depth)
                    {
                        distanceToWall += 0.1;

                        int testX = (int)(_playerX + rayX * distanceToWall);
                        int testY = (int)(_playerY + rayY * distanceToWall);

                        if (testX < 0 || testX >= Depth + _playerX || testY < 0 || testY >= Depth + _playerY)
                        {
                            hitWall = true;
                            distanceToWall = Depth;
                        }
                        else
                        {
                            char testCell = _map[testY * MapWidth + testX];

                            if (testCell == '#')
                            {
                                hitWall = true;
                            }
                        }
                    }

                    int ceiling = (int)(ScreenHeight / 2d - ScreenHeight * Fov / distanceToWall);
                    int floor = ScreenHeight - ceiling;

                    for (int y = 0; y < ScreenHeight; y++)
                    {
                        if (y <= ceiling)
                        {
                            Screen[y * ScreenWidth + x] = ' ';
                        }
                        else if (y > ceiling && y <= floor)
                        {
                            Screen[y * ScreenWidth + x] = '#';
                        }
                        else
                        {
                            Screen[y * ScreenWidth + x] = '.';
                        }
                    }
                }

                Console.SetCursorPosition(0, 0);
                Console.Write(Screen);
            }
        }
    }
}
