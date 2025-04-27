using System.Runtime.InteropServices;

namespace ConsoleGame
{
    public class MouseController
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT { public int X; public int Y; }

        private readonly int _centerX;
        private readonly int _centerY;

        private POINT _lastMousePos;

        public MouseController(int screenWidth, int screenHeight)
        {
            _centerX = screenWidth / 2;
            _centerY = screenHeight / 2;
            SetCursorPos(_centerX, _centerY);
            GetCursorPos(out _lastMousePos);
        }

        public void UpdatePlayerRotation(Player player, double elapsedTime)
        {
            GetCursorPos(out POINT currentPos);

            if (currentPos.X != _lastMousePos.X)
            {
                double sensitivity = 0.001;
                player.Angle -= (currentPos.X - _lastMousePos.X) * sensitivity;
                SetCursorPos(_centerX, _centerY);
            }

            _lastMousePos = currentPos;
        }
    }
}
