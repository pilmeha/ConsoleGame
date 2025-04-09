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
                    //x *= aspect * pixelAspect;
                    char pixel = ' ';
                    if (x * x + y * y < 0.5) pixel = '@';
                    Screen[i + j * ScreenWidth] = pixel;
                }
            }
            Console.SetCursorPosition(0, 0);
            Console.Write(Screen);
        }
    }
}
