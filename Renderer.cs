namespace ConsoleGame
{
    public class Renderer
    {
        private readonly int _screenWidth;
        private readonly int _screenHeight;
        private readonly Map _map;
        private readonly Player _player;

        private const double Fov = Math.PI / 3;
        private const double Depth = 16;

        public Renderer(int screenWidth, int screenHeight, Map map, Player player)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _map = map;
            _player = player;
        }

        public async Task RenderAsync(char[] screen)
        {
            var tasks = new List<Task<Dictionary<int, char>>>();

            for (int x = 0; x < _screenWidth; x++)
            {
                int col = x;
                tasks.Add(Task.Run(() => CastRay(col)));
            }

            var rays = await Task.WhenAll(tasks);

            foreach (var ray in rays)
            {
                foreach (var (index, symbol) in ray)
                    screen[index] = symbol;
            }

            DrawMap(screen);
            DrawPlayer(screen);
        }

        private Dictionary<int, char> CastRay(int x)
        {
            var result = new Dictionary<int, char>();

            double rayAngle = _player.Angle + Fov / 2 - x * Fov / _screenWidth;

            double rayX = Math.Sin(rayAngle);
            double rayY = Math.Cos(rayAngle);

            double distanceToWall = 0;
            bool hitWall = false;

            while (!hitWall && distanceToWall < Depth)
            {
                distanceToWall += 0.1;
                int testX = (int)(_player.X + rayX * distanceToWall);
                int testY = (int)(_player.Y + rayY * distanceToWall);

                if (testX < 0 || testX >= Map.Width || testY < 0 || testY >= Map.Height || _map.GetTile(testX, testY) == '#')
                    hitWall = true;
            }

            int ceiling = (int)(_screenHeight / 2.0 - _screenHeight * Fov / distanceToWall);
            int floor = _screenHeight - ceiling;

            for (int y = 0; y < _screenHeight; y++)
            {
                if (y <= ceiling)
                    result[y * _screenWidth + x] = ' ';
                else if (y > ceiling && y <= floor)
                    result[y * _screenWidth + x] = '\u2588';
                else
                    result[y * _screenWidth + x] = '.';
            }

            return result;
        }

        private void DrawMap(char[] screen)
        {
            for (int y = 0; y < Map.Height; y++)
                for (int x = 0; x < Map.Width; x++)
                    screen[(y + 1) * _screenWidth + x] = _map.GetTile(x, y);
        }

        private void DrawPlayer(char[] screen)
        {
            int px = (int)_player.X;
            int py = (int)_player.Y;
            screen[(py + 1) * _screenWidth + px] = 'P';
        }
    }
}
