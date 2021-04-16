using System;
using System.Drawing;

namespace DrMario.Implementations
{
    public static class GameToDrawingColorConvert
    {
        private static Color defaultColor = Color.FromArgb(100, 0, 0, 250);

        public static Color ToDrawingColor(GameCellColor cellColor)
        {
            if (cellColor == GameCellColor.None) return defaultColor;
            if (cellColor == GameCellColor.Red) return Color.Red;
            if (cellColor == GameCellColor.Yellow) return Color.Yellow;
            if (cellColor == GameCellColor.Blue) return Color.Blue;
            throw new ArgumentException("Некорректный цвет");
        }
    }
}
