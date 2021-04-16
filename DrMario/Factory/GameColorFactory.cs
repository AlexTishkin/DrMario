using System;

namespace DrMario.Implementations
{
    public static class GameColorFactory
    {
        private static Random _random = new Random();

        public static GameCellColor CreateRandomColor()
        {
            return (GameCellColor)_random.Next(1, 4);
        }
    }
}
