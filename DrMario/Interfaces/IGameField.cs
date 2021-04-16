using System.Collections.Generic;

namespace DrMario.Interfaces
{
    /// <summary>
    /// Игровое поле
    /// </summary>
    public interface IGameField
    {
        int Height { get; }
        int Width { get; }

        IGameCell this[int row, int col] { get; set; }

        /// <summary>
        /// Оставшиеся вирусы
        /// </summary>
        int LeftViruses { get; }

        /// <summary>
        /// Генерация поля
        /// </summary>
        void Initialize();

        /// <summary>
        /// Генерация вирусов
        /// </summary>
        void InitializeViruses(int count);

        /// <summary>
        /// Проверка игрового поля:
        /// - Уничтожение собранных вирусов
        /// - Обвал полуразрушенных блоков
        /// - Победа / Поражение
        /// </summary>
        IList<IGameCell> GetRemoveCells();

        void RemoveCells(IList<IGameCell> cells);

        /// <summary>
        /// Падающие блоки
        /// </summary>
        /// <returns></returns>
        IList<IGameCell> GetFallingCells(IList<IGameCell> fallingCells);
        void FallSingleCells(IList<IGameCell> fallingBlocks);
    }
}
