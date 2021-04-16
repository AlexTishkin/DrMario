using DrMario.Extensions;
using DrMario.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DrMario.Implementations
{
    public class GameField : IGameField
    {
        private IGameCell[,] _gameCells;

        public int Height => 16;
        public int Width => 8;

        public IGameCell this[int row, int col]
        {
            get => _gameCells[row, col];
            set => _gameCells[row, col] = value;
        }

        public int LeftViruses
        {
            get
            {
                var viruses = 0;
                for (var row = 0; row < Height; row++)
                    for (var col = 0; col < Width; col++)
                        if (_gameCells[row, col].Type == GameCellType.Virus)
                            viruses++;
                return viruses;
            }
        }

        public GameField()
        {
            _gameCells = new IGameCell[Height, Width];
            Initialize();
        }

        public void Initialize()
        {
            for (var row = 0; row < Height; row++)
                for (var col = 0; col < Width; col++)
                    _gameCells[row, col] = new GameCell(row, col);
        }

        public void InitializeViruses(int count)
        {
            int[] widthViruses = new[] { 0, 1, 2, 3, 4, 5, 6, 7 }.OrderBy(n => Guid.NewGuid()).Take(count).ToArray();
            int[] heightViruses = new[] { 8, 9, 10, 11, 12, 13, 14, 15 }.OrderBy(n => Guid.NewGuid()).Take(count).ToArray();

            for (var i = 0; i < widthViruses.Length; i++)
            {
                int row = heightViruses[i];
                int col = widthViruses[i];
                this[row, col] = VirusFactory.CreateRandomVirus(row, col);
            }
        }

        public IList<IGameCell> GetRemoveCells()
        {
            var removingCells = new List<IGameCell>();

            for (var row = 0; row < Height; row++)
            {
                var removingRowCells = GetRemoveCells(_gameCells.GetRow(row));
                removingCells.AddRange(removingRowCells);
            }

            for (var col = 0; col < Width; col++)
            {
                var removingColumnCells = GetRemoveCells(_gameCells.GetCol(col));
                removingCells.AddRange(removingColumnCells);
            }

            return removingCells;
        }

        private List<IGameCell> GetRemoveCells(IList<IGameCell> cells)
        {
            var removingCells = new List<IGameCell>();

            var tempRemovingCells = new Stack<IGameCell>();
            GameCellColor? tempColor = null;

            for (var i = 0; i < cells.Count; i++)
            {
                if (cells[i].Type != GameCellType.None)
                {
                    tempColor = tempColor ?? cells[i].Color;

                    if (cells[i].Color == tempColor)
                    {
                        tempRemovingCells.Push(cells[i]);
                    }
                    else
                    {
                        if (tempRemovingCells.Count >= 4)
                            removingCells.AddRange(tempRemovingCells);
                        tempRemovingCells.Clear();

                        tempColor = cells[i].Color;
                        tempRemovingCells.Push(cells[i]);
                    }
                }

                var isLastOrNone = (cells[i].Type == GameCellType.None) || (i + 1 == cells.Count);

                if (isLastOrNone)
                {
                    if (tempRemovingCells.Count >= 4)
                        removingCells.AddRange(tempRemovingCells);
                    tempRemovingCells.Clear();

                    tempColor = null;
                }
            }

            return removingCells;
        }

        public void RemoveCells(IList<IGameCell> cells)
        {
            for (int i = 0; i < cells.Count; i++)
                cells[i].Clear();
        }

        /// <summary>
        /// Получить список падающих блоков
        /// </summary>
        /// <returns></returns>
        public IList<IGameCell> GetFallingCells(IList<IGameCell> fallingCells)
        {
            var resultFallingCells = new List<IGameCell>();

            // Игровая сетка
            for (var row = 0; row < Height; row++)
                for (var col = 0; col < Width; col++)
                    if (_gameCells[row, col].CanFallDownSingleBlock(this))
                    {
                        resultFallingCells.Add((IGameCell)_gameCells[row, col].Clone());
                        _gameCells[row, col].Clear();
                    }

            // Зависшие блоки
            if (fallingCells != null && fallingCells.Count() > 0)
            {
                foreach (var fallingCell in fallingCells)
                {
                    if (fallingCell.CanFallDownSingleBlock(this))
                        resultFallingCells.Add(fallingCell);
                }
            }

            return resultFallingCells.Count() == 0 ? null : resultFallingCells;
        }

        /// <summary>
        /// Падение одиночных блоков (если имеются не закрепленные)
        /// </summary>
        public void FallSingleCells(IList<IGameCell> fallingBlocks)
        {
            foreach (var block in fallingBlocks)
            {
                if (block.CanFallDownSingleBlock(this))
                {
                    block.FallDown();

                    // Слепок на сетке 
                    if (!block.CanFallDownSingleBlock(this))
                    {
                        this[block.Row, block.Column] = block;
                    }

                }
            }
        }
    }
}
