using DrMario.Extensions;
using System;
using System.Drawing;

namespace DrMario.Factory
{
    public class VirusImageFactory
    {
        private readonly string _imagesFolderPath;
        private readonly Image _yellowVirus;
        private readonly Image _blueVirus;
        private readonly Image _redVirus;

        private readonly int _cellSize;

        public VirusImageFactory(int cellSize)
        {
            _cellSize = cellSize;
            _imagesFolderPath = @"C:\Users\mi\Desktop\DrMario\DrMario\Images\";
            _yellowVirus = Image.FromFile(_imagesFolderPath + "virus_yellow.png").ResizeImage(_cellSize, _cellSize);
            _blueVirus = Image.FromFile(_imagesFolderPath + "virus_blue.png").ResizeImage(_cellSize, _cellSize);
            _redVirus = Image.FromFile(_imagesFolderPath + "virus_red.png").ResizeImage(_cellSize, _cellSize);
        }

        public Image CreateVirusImage(GameCellColor color)
        {
            if (color == GameCellColor.Yellow) return _yellowVirus;
            if (color == GameCellColor.Blue) return _blueVirus;
            if (color == GameCellColor.Red) return _redVirus;
            throw new ArgumentException("Incorrect color (GameCellColor)");
        }
    }
}
