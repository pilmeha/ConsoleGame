//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System;

namespace ConsoleGame
{
    class Program
    {
        
        private const int ScreenWidth = 100;
        private const int ScreenHeight = 50;

        private const float aspect = (float)(ScreenWidth / ScreenHeight);
        private const float pixelAspect = 11.0f / 24.0f;

        private static readonly char[] Screen = new char[ScreenWidth * ScreenHeight];

        static void Main(string[] args)
        {
            Console.SetWindowSize(ScreenWidth, ScreenHeight);
            Console.SetBufferSize(ScreenWidth, ScreenHeight);
            Console.CursorVisible = false;

            for (int i = 0; i < ScreenWidth; i++)
            {
                for (int j = 0; j < ScreenHeight; j++)
                {
                    float x = (float)i / ScreenWidth * 2.0f - 1.0f;
                    float y = (float)j / ScreenHeight * 2.0f - 1.0f;
                    x *= aspect * pixelAspect;
                    char pixel = ' ';
                    if (x * x + y * y < 0.5) pixel = '@';
                    Screen[i + j * ScreenWidth] = pixel;
                }
            }


            //while (true)
            //{
            //    //if (Console.KeyAvailable)
            //    //{
            //    //    c = Console.ReadKey().KeyChar;
            //    //}

            //    //Array.Fill(Screen, c);
            //    for (int x = 0; x < ScreenWidth; x++)
            //    {
            //        double rayAngle = _playerA + Fov / 2 - x * Fov / ScreenWidth;

            //        double rayX = Math.Sin(rayAngle);
            //        double rayY = Math.Cos(rayAngle);

            //        double distanceToWall = 0;
            //        bool hitWall = false;

            //        while (!hitWall && distanceToWall < Depth)
            //        {
            //            distanceToWall += 0.1;

            //            int testX = (int)(_playerX + rayX * distanceToWall);
            //            int testY = (int)(_playerY + rayY * distanceToWall);

            //            if (testX < 0 || testX >= Depth + _playerX || testY < 0 || testY >= Depth + _playerY)
            //            {
            //                hitWall = true;
            //                distanceToWall = Depth;
            //            }
            //            else
            //            {
            //                char testCell = _map[testY * MapWidth + testX];

            //                if (testCell == '#')
            //                {
            //                    hitWall = true;
            //                }
            //            }
            //        }

            //        int ceiling = (int)(ScreenHeight / 2d - ScreenHeight * Fov / distanceToWall);
            //        int floor = ScreenHeight - ceiling;

            //        for (int y = 0; y < ScreenHeight; y++)
            //        {
            //            if (y <= ceiling)
            //            {
            //                Screen[y * ScreenWidth + x] = ' ';
            //            }
            //            else if (y > ceiling && y <= floor)
            //            {
            //                Screen[y * ScreenWidth + x] = '#';
            //            }
            //            else
            //            {
            //                Screen[y * ScreenWidth + x] = '.';
            //            }
            //        }
            //    }

            //    Console.SetCursorPosition(0, 0);
            //    Console.Write(Screen);
            //}
            Console.SetCursorPosition(0, 0);
            Console.Write(Screen);
        }
    }
}
