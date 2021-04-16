using DrMario.Implementations;
using DrMario.Interfaces;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DrMario.Display
{
    public class DisplayGrid : IDisplayGrid
    {
        private readonly PictureBox _window;
        private readonly int _cellSize;

        public Control this[int row, int col]
        {
            get
            {
                return _window.Controls.Find($"[{row},{col}]", false).First();
            }
            set
            {
                value.Top = GetXCoordinateByRow(row);
                value.Left = GetYCoordinateByCol(col);
                value.Size = new Size(_cellSize - 1, _cellSize - 1);
                value.Name = $"[{row},{col}]";
                _window.Controls.Add(value);
            }
        }

        public DisplayGrid(PictureBox window, int cellSize, IGameField gameField)
        {
            _window = window;
            _cellSize = cellSize;
            InitializeGrid(gameField);
            InitializeNextBlock();
        }

        private void InitializeGrid(IGameField gameField)
        {
            for (var row = 0; row < gameField.Height; row++)
                for (var col = 0; col < gameField.Width; col++)
                    this[row, col] = new PictureBox { BackColor = GameToDrawingColorConvert.ToDrawingColor(GameCellColor.None) };
        }

        /// <summary>
        /// Инициализация следующего блока
        /// </summary>
        private void InitializeNextBlock()
        {
            this[1, 10] = new PictureBox { BackColor = GameToDrawingColorConvert.ToDrawingColor(GameCellColor.None) };
            this[1, 11] = new PictureBox { BackColor = GameToDrawingColorConvert.ToDrawingColor(GameCellColor.None) };
        }

        /// <summary>
        /// Возвращает координаты X окна по номеру ячейки
        /// </summary>
        private int GetXCoordinateByRow(int row) => row * (_cellSize + 1) + 30;

        /// <summary>
        /// Возвращает координаты Y окна по номеру ячейки
        /// </summary>
        private int GetYCoordinateByCol(int row) => row * (_cellSize + 1) + 230;
    }
}
