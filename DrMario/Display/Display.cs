using DrMario.Extensions;
using DrMario.Factory;
using DrMario.Interfaces;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DrMario.Display
{
    public class Display : IDisplay
    {
        private readonly IGame _game;
        private readonly IDisplayGrid _gameGrid;

        private readonly int _nextBlockRow = 1;
        private readonly int _nextBlockCol = 10;

        private readonly VirusImageFactory _virusImageFactory;

        /// <summary>
        /// Высота и ширина ячейки (квадрат)
        /// </summary>
        private readonly int _cellSize = 36;

        private readonly PictureBox _window;

        public Display(IGame game, PictureBox window)
        {
            _game = game;
            _window = window;
            _gameGrid = new DisplayGrid(window, _cellSize, _game.Field);
            _virusImageFactory = new VirusImageFactory(_cellSize);
        }

        public void ShowGameState()
        {
            DisplayGridCell(_game.Field);
            DisplayNextBlock(_game.NextBlock);
            DisplayFallingBlock(_game.FallingBlock);
            DisplayFallingCells(_game.FallingCells);
            _window.Refresh();
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
                    {
                        _gameGrid[row, col].SetColor(gameField[row, col].Color);
                    }

                    if (gameField[row, col].Type == GameCellType.Virus)
                    {
                        var virusImage = _virusImageFactory.CreateVirusImage(gameField[row, col].Color);
                        _gameGrid[row, col].SetColor(GameCellColor.None);
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
    }
}
