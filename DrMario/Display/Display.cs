using DrMario.Extensions;
using DrMario.Interfaces;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace DrMario.Display
{
    public class Display : IDisplay
    {
        private readonly IGame _game;
        private readonly IDisplayGrid _gameGrid;

        private readonly int _nextBlockRow = 1;
        private readonly int _nextBlockCol = 10;

        private readonly Image _yellowVirus;
        private readonly Image _blueVirus;
        private readonly Image _redVirus;

        /// <summary>
        /// Высота и ширина ячейки (квадрат)
        /// </summary>
        //private readonly int _cellSize = 29;
        private readonly int _cellSize = 36;

        public Display(IGame game, PictureBox window)
        {
            _game = game;
            _gameGrid = new DisplayGrid(window, _cellSize, _game.Field);

            var imagesFolder = @"C:\Users\mi\Desktop\DrMario\DrMario\Images\";

            _yellowVirus = ResizeImage(Image.FromFile(imagesFolder + "virus_yellow.png"), _cellSize, _cellSize);
            _blueVirus = ResizeImage(Image.FromFile(imagesFolder + "virus_blue.png"), _cellSize, _cellSize);
            _redVirus = ResizeImage(Image.FromFile(imagesFolder + "virus_red.png"), _cellSize, _cellSize);
        }

        public void ShowGameState()
        {
            DisplayGridCell(_game.Field);
            DisplayNextBlock(_game.NextBlock);
            DisplayFallingBlock(_game.FallingBlock);

            DisplayFallingCells(_game.FallingCells);
        }

        /// <summary>
        /// Отрисовка игровой сетки
        /// </summary>
        private void DisplayGridCell(IGameField gameField)
        {
            for (var row = 0; row < gameField.Height; row++)
                for (var col = 0; col < gameField.Width; col++)
                {
                    ((PictureBox)_gameGrid[row, col]).Image = null;

                    if (gameField[row, col].Type != GameCellType.Virus)
                        _gameGrid[row, col].SetColor(gameField[row, col].Color);

                    if (gameField[row, col].Type == GameCellType.Virus)
                    {
                        // TODO: To Factory...
                        Image virusImage = null;
                        if (gameField[row, col].Color == GameCellColor.Yellow)
                            virusImage = _yellowVirus;
                        else if (gameField[row, col].Color == GameCellColor.Blue)
                            virusImage = _blueVirus;
                        else if (gameField[row, col].Color == GameCellColor.Red)
                            virusImage = _redVirus;

                        ((PictureBox)_gameGrid[row, col]).Image = virusImage;
                    }
                }
        }

        /// <summary>
        /// Отрисовка следующего блока
        /// </summary>
        private void DisplayNextBlock(IGameBlock nextBlock)
        {
            _gameGrid[_nextBlockRow, _nextBlockCol].SetColor(nextBlock.Left.Color);
            _gameGrid[_nextBlockRow, _nextBlockCol + 1].SetColor(nextBlock.Right.Color);
        }

        /// <summary>
        /// Отрисовка следующего блока
        /// </summary>
        private void DisplayFallingBlock(IGameBlock fallingBlock)
        {
            if (fallingBlock == null) return;
            _gameGrid[fallingBlock.Left.Row, fallingBlock.Left.Column].SetColor(fallingBlock.Left.Color);
            _gameGrid[fallingBlock.Right.Row, fallingBlock.Right.Column].SetColor(fallingBlock.Right.Color);
        }

        private void DisplayFallingCells(IList<IGameCell> cells)
        {
            if (cells == null) return;

            foreach (var cell in cells)
                _gameGrid[cell.Row, cell.Column].SetColor(cell.Color);
        }

        // Utility

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        //

    }
}
