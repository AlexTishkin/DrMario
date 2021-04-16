using System.Collections.Generic;
using System.Windows.Forms;

namespace DrMario.Interfaces
{
    public interface IGame
    {
        /// <summary>
        /// Игровой такт
        /// </summary>
        void GameTact();

        /// <summary>
        /// Управление
        /// </summary>
        void OnUserAction(GameKey key);

        // Состояние игры //

        /// <summary>
        /// Игровое поле
        /// </summary>
        IGameField Field { get; }

        /// <summary>
        /// Следующий блок
        /// </summary>
        IGameBlock NextBlock { get; set; }

        /// <summary>
        /// Падающий блок
        /// </summary>
        IGameBlock FallingBlock { get; set; }

        /// <summary>
        /// Падающие остатки блоков
        /// </summary>
        IList<IGameCell> FallingCells { get; set; }
    }
}
