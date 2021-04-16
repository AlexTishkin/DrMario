using System;

namespace DrMario.Interfaces
{
    /// <summary>
    /// Игровая ячейка
    /// </summary>
    public interface IGameCell : ICloneable
    {
        GameCellType Type { get; set; }
        GameCellColor Color { get; set; }

        /// <summary>
        /// Позиция по длине
        /// </summary>
        int Row { get; set; }

        /// <summary>
        /// Позиция по высоте
        /// </summary>
        int Column { get; set; }

        /// <summary>
        /// Связанный блок
        /// </summary>
        IGameCell BindCell { get; set; }

        /// <summary>
        /// Очистить клетку
        /// </summary>
        void Clear();

        /// <summary>
        /// Может ли ячейка падать
        /// </summary>
        bool CanFallDown(IGameField gameField);

        /// <summary>
        /// Падение ячейки вниз
        /// </summary>
        void FallDown();

        /// <summary>
        /// Падение одинокой ячейки вниз (обвал блоков)
        /// </summary>
        bool CanFallDownSingleBlock(IGameField gameField);
    }
}