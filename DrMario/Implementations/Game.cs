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
        /// Падение одиночных ячеек
        /// </summary>
        public IList<IGameCell> FallingCells { get; set; }

        private int VirusCount { get; set; }

        /// <summary>
        /// Идет игра!
        /// </summary>
        private bool IsGame { get; set; }

        public Game()
        {
            VirusCount = 4;
            Field = new GameField();
            Field.InitializeViruses(VirusCount);
            IsGame = true;
            InitializeNextBlock();
        }

        private void InitializeNextBlock()
        {
            NextBlock = new GameBlock(1, 10);
            NextBlock.SetRandomColor();
        }

        public void GameTact()
        {
            // Игра не началась!
            if (!IsGame)
                return;

            // Событие - победа
            if (IsGame && Field.LeftViruses == 0)
            {
                IsGame = false;
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
                    IsGame = false;
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

        // /// //// ////////////////////////////////////////

        public void OnUserAction(GameKey key)
        {
            // Победа / Поражение - Начать игру [ПЛОХАААА]
            if (!IsGame)
            {
                FallingBlock = null;
                FallingCells = null;
                IsGame = true;

                Field = new GameField();
                Field.InitializeViruses(++VirusCount);
                InitializeNextBlock();

            }

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
