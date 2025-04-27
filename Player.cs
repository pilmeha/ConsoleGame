namespace ConsoleGame
{
    public class Player
    {
        private readonly Map _map;

        public double X { get; private set; } = 5;
        public double Y { get; private set; } = 5;
        public double Angle { get; set; } = 0;

        public Player(Map map)
        {
            _map = map;
            PlaceOnEmptySpot();
        }

        public void PlaceOnEmptySpot()
        {
            Random random = new Random();
            do
            {
                X = random.Next(1, Map.Width - 1);
                Y = random.Next(1, Map.Height - 1);
            } while (_map.GetTile((int)X, (int)Y) != '.');
        }

        public void HandleMovement(ConsoleKey key, double elapsedTime)
        {
            double moveSpeed = 25 * elapsedTime;
            double strafeSpeed = 20 * elapsedTime;

            switch (key)
            {
                case ConsoleKey.W: Move(Math.Sin(Angle), Math.Cos(Angle), moveSpeed); break;
                case ConsoleKey.S: Move(-Math.Sin(Angle), -Math.Cos(Angle), moveSpeed); break;
                case ConsoleKey.A: Move(Math.Sin(Angle + Math.PI / 2), Math.Cos(Angle + Math.PI / 2), strafeSpeed); break;
                case ConsoleKey.D: Move(Math.Sin(Angle - Math.PI / 2), Math.Cos(Angle - Math.PI / 2), strafeSpeed); break;
            }
        }

        private void Move(double dx, double dy, double speed)
        {
            double newX = X + dx * speed;
            double newY = Y + dy * speed;

            if (_map.GetTile((int)newX, (int)newY) != '#')
            {
                X = newX;
                Y = newY;
            }
        }
    }
}
