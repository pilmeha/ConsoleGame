using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    public class Game
    {
        private readonly Map _map;
        private readonly Player _player;
        private readonly Renderer _renderer;
        private readonly MouseController _mouseController;

        private const int ScreenWidth = 150;
        private const int ScreenHeight = 50;

        private char[] _screen;
        private DateTime _lastFrameTime;

        public Game()
        {
            Console.SetWindowSize(ScreenWidth, ScreenHeight);
            Console.SetBufferSize(ScreenWidth, ScreenHeight);
            Console.CursorVisible = false;

            _screen = new char[ScreenWidth * ScreenHeight];
            _map = new Map();
            _player = new Player(_map);
            _renderer = new Renderer(ScreenWidth, ScreenHeight, _map, _player);
            _mouseController = new MouseController(ScreenWidth, ScreenHeight);

            _lastFrameTime = DateTime.Now;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                double elapsedTime = (DateTime.Now - _lastFrameTime).TotalSeconds;
                _lastFrameTime = DateTime.Now;

                _mouseController.UpdatePlayerRotation(_player, elapsedTime);
                HandleInput(elapsedTime);

                await _renderer.RenderAsync(_screen);

                Console.SetCursorPosition(0, 0);
                Console.Write(_screen);
            }
        }

        private void HandleInput(double elapsedTime)
        {
            if (!Console.KeyAvailable) return;

            var key = Console.ReadKey(true).Key;

            _player.HandleMovement(key, elapsedTime);

            if (key == ConsoleKey.R)
            {
                _map.GenerateRandom();
                _player.PlaceOnEmptySpot();
            }
            else if (key == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }
        }
    }
}
