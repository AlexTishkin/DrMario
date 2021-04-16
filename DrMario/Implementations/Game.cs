using DrMario.Interfaces;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DrMario.Implementations
{
    public class Game : IGame
    {
        /// <summary>
        /// Игровое поле
        /// </summary>
        public IGameField Field { get; private set; }

        /// <summary>
        /// Следующий блок
        /// </summary>
        public IGameBlock NextBlock { get; set; }

        /// <summary>
        /// Падающий блок
        /// </summary>
        public IGameBlock FallingBlock { get; set; }

        /// <summary>
        /// Падающие одиночные ячейки
        /// </summary>
        public IList<IGameCell> FallingCells { get; set; }

        private int VirusCount { get; set; }

        /// <summary>
        /// Идет игра!
        /// </summary>
        private bool IsGameRunning { get; set; }

        public Game()
        {
            RunNewGame(virusCount: 2);
        }

        public void RunNewGame(int virusCount)
        {
            Field = new GameField();

            VirusCount = virusCount;
            Field.InitializeViruses(VirusCount);

            InitializeNextBlock();

            FallingBlock = null;
            FallingCells = null;
            IsGameRunning = true;
        }

        private void InitializeNextBlock()
        {
            NextBlock = new GameBlock(1, 10);
            NextBlock.SetRandomColor();
        }

        public void GameTact()
        {
            if (!IsGameRunning) return;

            // Событие - победа
            if (IsGameRunning && Field.LeftViruses == 0)
            {
                IsGameRunning = false;
                MessageBox.Show("Победа! Нажмите любую клавишу для продолжения!");
                return;
            }

            // Падение одиночных блоков
            if (FallingCells != null)
            {
                FallSingleCells();
                //return;
            }

            // Появление следующего блока
            if (FallingBlock == null)
            {
                // Чек -> Или поражение!
                if (Field[0, 3].Type != GameCellType.None || Field[0, 4].Type != GameCellType.None)
                {
                    IsGameRunning = false;
                    MessageBox.Show("Поражение! Нажмите любую клавишу для продолжения!");
                    return;
                }

                AddNextBlockToGrid();
                return;
            }

            // Управление летящим блоком
            if (FallingBlock.CanFallDown(Field))
            {
                FallBlock();
                return;
            }

            FellBlock();
        }

        // Различные режимы игрового такта

        /// <summary>
        /// Добавление нового игрового блока на сетку
        /// </summary>
        private void AddNextBlockToGrid()
        {
            FallingBlock = new GameBlock(NextBlock);
            NextBlock.SetRandomColor();
        }

        /// <summary>
        /// Падение нового блока
        /// </summary>
        private void FallBlock()
        {
            FallingBlock.FallDown(Field);
        }

        /// <summary>
        /// Блок упал
        /// </summary>
        private void FellBlock()
        {
            // Летящий блок приземлился
            // Сохранение упавшего блока в таком положении на сетке
            FallingBlock.Type = GameCellType.Block;
            Field[FallingBlock.Left.Row, FallingBlock.Left.Column] = FallingBlock.Left;
            Field[FallingBlock.Right.Row, FallingBlock.Right.Column] = FallingBlock.Right;
            FallingBlock = null;

            var removingCells = Field.GetRemoveCells();
            Field.RemoveCells(removingCells);

            // Падающие блоки
            FallingCells = Field.GetFallingCells(FallingCells);

            // TODO: Проверка на разрушенные блоки
            // TODO: Проверка падающих блоков
            // Циклично //
            //}
        }

        /// <summary>
        /// Падение одиночных блоков - после разрушения
        /// </summary>
        private void FallSingleCells()
        {
            Field.FallSingleCells(FallingCells);
            FallingCells = Field.GetFallingCells(FallingCells);
            // Опустошить падающие блоки -> Превратить в список динамических блоков
            // Падение динамических блоков
            // Проверка
            // Повторение!
        }

        public void OnUserAction(GameKey key)
        {
            // Следующая игра (Нажатие любой клавиши)
            if (!IsGameRunning)
                RunNewGame(++VirusCount);

            if (key == GameKey.NONE || FallingBlock == null)
                return;

            if (key == GameKey.LEFT)
            {
                if (FallingBlock.CanMove(GameSide.Left, Field))
                    FallingBlock.Move(GameSide.Left);
            }

            if (key == GameKey.RIGHT)
            {
                if (FallingBlock.CanMove(GameSide.Right, Field))
                    FallingBlock.Move(GameSide.Right);
            }

            if (key == GameKey.DOWN)
            {
                if (FallingBlock.CanMove(GameSide.Down, Field))
                    FallingBlock.Move(GameSide.Down);
            }

            if (key == GameKey.SPACE)
            {
                if (FallingBlock.CanTurn(Field))
                    FallingBlock.Turn();
            }
        }

    }
}
