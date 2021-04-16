using DrMario.Interfaces;
using System;
using System.Linq;

namespace DrMario.Implementations
{
    public class GameCell : IGameCell, ICloneable
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public GameCellType Type { get; set; }
        public GameCellColor Color { get; set; }

        public IGameCell BindCell { get; set; }

        public GameCell(int rowIndex, int columnIndex, GameCellType type = GameCellType.None, GameCellColor color = GameCellColor.None, IGameCell bindCell = null)
        {
            Row = rowIndex;
            Column = columnIndex;
            Type = type;
            Color = color;
            BindCell = bindCell;
        }

        public GameCell(GameCell cell)
        {
            Row = cell.Row;
            Column = cell.Column;
            Type = cell.Type;
            Color = cell.Color;
            BindCell = cell.BindCell;
        }

        public void Clear()
        {
            Type = GameCellType.None;
            Color = GameCellColor.None;

            // Связанный блок
            if (BindCell != null)
                BindCell.BindCell = null;
            BindCell = null;
        }

        public bool CanFallDown(IGameField gameField)
        {
            if (Row + 1 == gameField.Height)
                return false;

            return gameField[Row + 1, Column].Type == GameCellType.None;
        }

        public bool CanFallDownSingleBlock(IGameField gameField)
        {
            if (Row + 1 == gameField.Height)
                return false;

            //// Для падения одиночных блоков (Если это пустой блок или вирус, то это к нему отношения не имеет)
            if (Type == GameCellType.None || Type == GameCellType.Virus)
                return false;

            //// Если снизу связанные блок, то может падать
            //if (BindCell != null && gameField[Row + 1, Column] == BindCell && CanFallDownAnotherCell(BindCell, gameField))
            //    return true;

            //// Снизу блок тоже падает
            //if (gameField[Row + 1, Column].Type != GameCellType.None && gameField[Row + 1, Column].CanFallDownSingleBlock(gameField))
            //    return true;

            if (gameField[Row + 1, Column].Type != GameCellType.None)
                return false;

            return BindCell == null || CanFallDownAnotherCell(BindCell, gameField);
        }

        // Костылёк (ANTI DRY!)
        private static bool CanFallDownAnotherCell(IGameCell cell, IGameField gameField)
        {
            if (cell.Row + 1 == gameField.Height)
                return false;

            //// Для падения одиночных блоков (Если это пустой блок или вирус, то это к нему отношения не имеет)
            if (cell.Type == GameCellType.None || cell.Type == GameCellType.Virus)
                return false;

            if (gameField[cell.Row + 1, cell.Column].Type != GameCellType.None)
                return false;

            return true;
        }

        public void FallDown()
        {
            Row++;
        }

        /// <summary>
        /// ICloneable implementation
        /// </summary>
        /// <returns></returns>
        public object Clone() => new GameCell(this);

    }
}
