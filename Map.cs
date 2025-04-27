namespace ConsoleGame
{
    public class Map
    {
        public const int Width = 33;
        public const int Height = 33;

        private readonly char[] _data = new char[Width * Height];
        private readonly Random _random = new Random();

        public Map()
        {
            GenerateRandom();
        }

        public void GenerateRandom()
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    _data[y * Width + x] = (x == 0 || y == 0 || x == Width - 1 || y == Height - 1) ? '#' : (_random.NextDouble() < 0.1 ? '#' : '.');
        }

        public char GetTile(int x, int y) => _data[y * Width + x];

        public void SetTile(int x, int y, char value) => _data[y * Width + x] = value;
    }
}
