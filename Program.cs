//namespace ConsoleGame
//{
//    internal class Program
//    {
//        static async Task Main(string[] args)
//        {
//            var game = new Game();
//            await game.RunAsync();
//        }
//    }
//}



//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System;
using System.Runtime.InteropServices;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleGame
{
    class Program
    {

        // Импорт функций для работы с мышью
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();


        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        private static void GenerateMaze()
        {
            // Инициализация карты сплошными стенами
            Map.Clear();
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    Map.Append('#');
                }
            }

            // Начальная точка для генерации (должна быть нечетной)
            int startX = 1;
            int startY = 1;

            // Запускаем рекурсивную генерацию
            CarvePassage(startX, startY);

            // Убедимся, что границы остаются стенами
            for (int x = 0; x < MapWidth; x++)
            {
                Map[x] = '#'; // Верхняя граница
                Map[(MapHeight - 1) * MapWidth + x] = '#'; // Нижняя граница
            }
            for (int y = 0; y < MapHeight; y++)
            {
                Map[y * MapWidth] = '#'; // Левая граница
                Map[y * MapWidth + MapWidth - 1] = '#'; // Правая граница
            }

            // Создаем вход и выход
            Map[1 * MapWidth + 1] = '.'; // Вход
            Map[(MapHeight - 2) * MapWidth + (MapWidth - 2)] = '.'; // Выход
        }

        private static void CarvePassage(int x, int y)
        {
            // Делаем текущую клетку проходом
            Map[y * MapWidth + x] = '.';

            // Создаем случайный порядок направлений
            var directions = new[]
            {
                new { dx = 0, dy = -2 }, // Вверх
                new { dx = 0, dy = 2 },  // Вниз
                new { dx = -2, dy = 0 }, // Влево
                new { dx = 2, dy = 0 }   // Вправо
            }.OrderBy(d => _random.Next()).ToList();

            foreach (var dir in directions)
            {
                int newX = x + dir.dx;
                int newY = y + dir.dy;

                if (newX > 0 && newX < MapWidth - 1 && newY > 0 && newY < MapHeight - 1 && Map[newY * MapWidth + newX] == '#')
                {
                    // Прорубаем стену между текущей и новой клеткой
                    Map[(y + dir.dy / 2) * MapWidth + (x + dir.dx / 2)] = '.';
                    CarvePassage(newX, newY);
                }
            }
        }

        private const int ScreenWidth = 150;
        private const int ScreenHeight = 50;

        private const int MapWidth = 33;
        private const int MapHeight = 33;

        private const double Fov = Math.PI / 3;
        private const double Depth = 16;

        private static double _playerX = 5;
        private static double _playerY = 5;
        private static double _playerA = 0;


        private static readonly StringBuilder Map = new StringBuilder();

        private static readonly char[] Screen = new char[ScreenWidth * ScreenHeight];

        private static readonly Random _random = new Random();

        static async Task Main(string[] args)
        {

            Console.SetWindowSize(ScreenWidth, ScreenHeight);
            Console.SetBufferSize(ScreenWidth, ScreenHeight);
            Console.CursorVisible = false;

            // Центрируем курсор мыши
            POINT center = new POINT { X = ScreenWidth / 2, Y = ScreenHeight / 2 };
            SetCursorPos(center.X, center.Y);

            //методы для разных режимов игры, раскоментируйте нужный, также нужно сделать в строках 220 и 230
            //InitMap();
            GenerateRandomMap();
            //GenerateMaze();
            EnsurePlayerPosition();

            DateTime dateTimeFrom = DateTime.Now;
            POINT lastMousePos = center;

            while (true)
            {
                DateTime dateTimeTo = DateTime.Now;
                double elapsedTime = (dateTimeTo - dateTimeFrom).TotalSeconds;
                dateTimeFrom = DateTime.Now;

                // Управление камерой мышью
                POINT currentMousePos;
                GetCursorPos(out currentMousePos);

                if (currentMousePos.X != lastMousePos.X)
                {
                    double mouseSensitivity = 0.001;
                    _playerA -= (currentMousePos.X - lastMousePos.X) * mouseSensitivity;

                    // Возвращаем курсор в центр
                    SetCursorPos(center.X, center.Y);
                    currentMousePos = center;
                }

                lastMousePos = currentMousePos;

                if (GetForegroundWindow() == GetConsoleWindow())
                {
                    // Только если окно консоли активно
                    GetCursorPos(out currentMousePos);

                    if (currentMousePos.X != lastMousePos.X)
                    {
                        double mouseSensitivity = 0.001;
                        _playerA -= (currentMousePos.X - lastMousePos.X) * mouseSensitivity;

                        // Возвращаем курсор в центр
                        SetCursorPos(center.X, center.Y);
                        currentMousePos = center;
                    }

                    lastMousePos = currentMousePos;
                }

                // Возвращаем курсор в центр
                SetCursorPos(center.X, center.Y);

                if (Console.KeyAvailable)
                {
                    var consoleKey = Console.ReadKey(true).Key;

                    switch (consoleKey)
                    {
                        //case ConsoleKey.A:
                        //    _playerA += 5 * elapsedTime;
                        //    break;

                        //case ConsoleKey.D:
                        //    _playerA -= 5 * elapsedTime;
                        //    break;

                        case ConsoleKey.A:
                            {
                                // Движение влево (перпендикулярно направлению взгляда)
                                _playerX += Math.Sin(_playerA + Math.PI / 2) * 20 * elapsedTime;
                                _playerY += Math.Cos(_playerA + Math.PI / 2) * 20 * elapsedTime;
                                if (Map[(int)_playerY * MapWidth + (int)_playerX] == '#')
                                {
                                    _playerX -= Math.Sin(_playerA + Math.PI / 2) * 20 * elapsedTime;
                                    _playerY -= Math.Cos(_playerA + Math.PI / 2) * 20 * elapsedTime;
                                }
                                break;
                            }

                        case ConsoleKey.D:
                            {
                                // Движение вправо (перпендикулярно направлению взгляда)
                                _playerX += Math.Sin(_playerA - Math.PI / 2) * 20 * elapsedTime;
                                _playerY += Math.Cos(_playerA - Math.PI / 2) * 20 * elapsedTime;
                                if (Map[(int)_playerY * MapWidth + (int)_playerX] == '#')
                                {
                                    _playerX -= Math.Sin(_playerA - Math.PI / 2) * 20 * elapsedTime;
                                    _playerY -= Math.Cos(_playerA - Math.PI / 2) * 20 * elapsedTime;
                                }
                                break;
                            }

                        case ConsoleKey.W:
                            {
                                _playerX += Math.Sin(_playerA) * 25 * elapsedTime;
                                _playerY += Math.Cos(_playerA) * 25 * elapsedTime;
                                if (Map[(int)_playerY * MapWidth + (int)_playerX] == '#')
                                {
                                    _playerX -= Math.Sin(_playerA) * 25 * elapsedTime;
                                    _playerY -= Math.Cos(_playerA) * 25 * elapsedTime;
                                }
                                break;
                            }

                        case ConsoleKey.S:
                            {
                                _playerX -= Math.Sin(_playerA) * 25 * elapsedTime;
                                _playerY -= Math.Cos(_playerA) * 25 * elapsedTime;
                                if (Map[(int)_playerY * MapWidth + (int)_playerX] == '#')
                                {
                                    _playerX += Math.Sin(_playerA) * 25 * elapsedTime;
                                    _playerY += Math.Cos(_playerA) * 25 * elapsedTime;
                                }
                                break;
                            }

                        case ConsoleKey.R:
                            //раскоментируйте нужный режим игры
                            GenerateRandomMap();
                            //GenerateMaze();
                            EnsurePlayerPosition();
                            break;

                        case ConsoleKey.Escape:
                            return; //выход из игры
                    }
                    //один из режимов игры, если что закоментируйте при изменение режима игры
                    //InitMap();

                }

                //Ray casting

                var rayCastingTasks = new List<Task<Dictionary<int, char>>>();

                for (int x = 0; x < ScreenWidth; x++)
                {
                    int x1 = x;
                    rayCastingTasks.Add(Task.Run(() => CastRay(x1)));
                }

                var rays = await Task.WhenAll(rayCastingTasks);

                foreach (var dictionary in rays)
                {
                    foreach (var key in dictionary.Keys)
                    {
                        Screen[key] = dictionary[key];
                    }
                }

                //stats
                char[] stats = $"X: {_playerX}, Y: {_playerY}, A: {_playerA}, FPS: {(int)(1 / elapsedTime)}".ToCharArray();
                stats.CopyTo(Screen, 0);

                //map
                for (int x = 0; x < MapWidth; x++)
                {
                    for (int y = 0; y < MapHeight; y++)
                    {
                        Screen[(y + 1) * ScreenWidth + x] = Map[y * MapWidth + x];
                    }
                }

                //player
                Screen[(int)(_playerY + 1) * ScreenWidth + (int)_playerX] = 'P';

                // Draw direction indicator (3 points for better visibility)
                for (int x = 0; x < ScreenWidth; x++)
                {
                    double rayAngle = _playerA + Fov / 2 - x * Fov / ScreenWidth;
                    for (int i = 1; i <= 15; i++) // Дальность направления
                    {
                        int dirX = (int)(_playerX + i * Math.Sin(rayAngle));
                        int dirY = (int)(_playerY + i * Math.Cos(rayAngle));

                        if (dirX >= 0 && dirX < MapWidth && dirY >= 0 && dirY < MapHeight)
                        {
                            if (Map[dirY * MapWidth + dirX] != '#')
                            {
                                Screen[(dirY + 1) * ScreenWidth + dirX] = '*';
                            }
                            else
                            {
                                break; // Не рисуем направление через стены
                            }
                        }
                    }
                }

                Console.SetCursorPosition(0, 0);
                Console.Write(Screen);
            }

        }

        private static void GenerateRandomMap()
        {
            Map.Clear();

            // Fill the map with empty spaces first
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    // Borders are always walls
                    if (x == 0 || y == 0 || x == MapWidth - 1 || y == MapHeight - 1)
                    {
                        Map.Append('#');
                    }
                    else
                    {
                        Map.Append('.');
                    }
                }
            }

            // Add random walls
            for (int y = 1; y < MapHeight - 1; y++)
            {
                for (int x = 1; x < MapWidth - 1; x++)
                {
                    // 30% chance to place a wall (adjust this value to change density)
                    if (_random.NextDouble() < 0.1)
                    {
                        Map[y * MapWidth + x] = '#';
                    }
                }
            }

            // Ensure the player starting area is clear
            for (int y = (int)_playerY - 2; y <= (int)_playerY + 2; y++)
            {
                for (int x = (int)_playerX - 2; x <= (int)_playerX + 2; x++)
                {
                    if (x > 0 && x < MapWidth - 1 && y > 0 && y < MapHeight - 1)
                    {
                        Map[y * MapWidth + x] = '.';
                    }
                }
            }
        }

        private static void EnsurePlayerPosition()
        {
            // Make sure player starts in an empty space
            while (Map[(int)_playerY * MapWidth + (int)_playerX] != '.')
            {
                _playerX = _random.Next(1, MapWidth - 1);
                _playerY = _random.Next(1, MapHeight - 1);
            }
        }

        public static Dictionary<int, char> CastRay(int x)
        {
            var result = new Dictionary<int, char>();

            double rayAngle = _playerA + Fov / 2 - x * Fov / ScreenWidth;

            double rayX = Math.Sin(rayAngle);
            double rayY = Math.Cos(rayAngle);

            double distanceToWall = 0;
            bool hitWall = false;
            bool isBound = false;

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
                    char testCell = Map[testY * MapWidth + testX];

                    if (testCell == '#')
                    {
                        hitWall = true;

                        var boundsVectorList = new List<(double module, double cos)>();

                        for (int tx = 0; tx < 2; tx++)
                        {
                            for (int ty = 0; ty < 2; ty++)
                            {
                                double vx = testX + tx - _playerX;
                                double vy = testY + ty - _playerY;

                                double vectorModule = Math.Sqrt(vx * vx + vy * vy);
                                double cosAngle = rayX * vx / vectorModule + rayY * vy / vectorModule;

                                boundsVectorList.Add((vectorModule, cosAngle));
                            }
                        }

                        boundsVectorList = boundsVectorList.OrderBy(v => v.module).ToList();

                        double boundAngle = 0.03 / distanceToWall;

                        if (Math.Acos(boundsVectorList[0].cos) < boundAngle ||
                            Math.Acos(boundsVectorList[1].cos) < boundAngle)
                            isBound = true;
                    }
                    //else
                    //{
                    //    //Screen[testY * MapWidth + testX] = '*';

                    //    Map[testY * MapWidth + testX] = '*';
                    //}
                }
            }

            int ceiling = (int)(ScreenHeight / 2d - ScreenHeight * Fov / distanceToWall);
            int floor = ScreenHeight - ceiling;

            char wallShade;
            if (isBound)
                wallShade = '|';
            else if (distanceToWall <= Depth / 4d)
                wallShade = '\u2588';
            else if (distanceToWall < Depth / 3d)
                wallShade = '\u2593';
            else if (distanceToWall < Depth / 2d)
                wallShade = '\u2592';
            else if (distanceToWall < Depth)
                wallShade = '\u2591';
            else
                wallShade = ' ';

            for (int y = 0; y < ScreenHeight; y++)
            {
                if (y <= ceiling)
                {
                    result[y * ScreenWidth + x] = ' ';
                }
                else if (y > ceiling && y <= floor)
                {
                    result[y * ScreenWidth + x] = wallShade;
                }
                else
                {
                    char floorShade;

                    double b = 1 - (y - ScreenHeight / 2d) / (ScreenHeight / 2d);

                    if (b < 0.25)
                        floorShade = '#';
                    else if (b < 0.5)
                        floorShade = 'x';
                    else if (b < 0.75)
                        floorShade = '-';
                    else if (b < 0.9)
                        floorShade = '.';
                    else
                        floorShade = ' ';

                    result[y * ScreenWidth + x] = floorShade;
                }
            }

            return result;
        }

        private static void InitMap()
        {
            Map.Clear();
            Map.Append("#################################");
            Map.Append("#.........#...........#.........#");
            Map.Append("#.........#...........#.........#");
            Map.Append("#.....................#.........#");
            Map.Append("#################.....#.........#");
            Map.Append("#.....................#.........#");
            Map.Append("#........#....#.......#.........#");
            Map.Append("#........#....#.......#..########");
            Map.Append("######...##########.............#");
            Map.Append("#....#...#......................#");
            Map.Append("#....#...#.......#########......#");
            Map.Append("#....#...#...............#......#");
            Map.Append("#....#...#############...#......#");
            Map.Append("#........................#......#");
            Map.Append("#..##############........#......#");
            Map.Append("#....#.....#.............#......#");
            Map.Append("#....#............########......#");
            Map.Append("#....#...................#......#");
            Map.Append("#....#.........#................#");
            Map.Append("#..............###########......#");
            Map.Append("#####...#......#................#");
            Map.Append("#.......#......#................#");
            Map.Append("#.......#......#................#");
            Map.Append("#.......#.......................#");
            Map.Append("#.......#.......................#");
            Map.Append("#.......#......#.....#..........#");
            Map.Append("#.......###.########.#..........#");
            Map.Append("#.......#....#.......#..#########");
            Map.Append("#.......#....#.......#..........#");
            Map.Append("#.......#....#.......#..........#");
            Map.Append("#............#######.#..........#");
            Map.Append("#.......#............#..........#");
            Map.Append("#################################");
        }
    }
}
