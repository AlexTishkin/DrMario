using DrMario.Interfaces;
using System;

namespace DrMario.Implementations
{
    public static class VirusFactory
    {
        public static IGameCell CreateRandomVirus(int row, int col)
        {
            var randomCellColor = GameColorFactory.CreateRandomColor();
            return new GameCell(row, col, GameCellType.Virus, randomCellColor);
        }
    }
}
