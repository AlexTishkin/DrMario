using DrMario.Interfaces;
using System;

namespace DrMario.Implementations
{
    public class GameBlock : IGameBlock
    {
        public IGameCell Left { get; set; }
        public IGameCell Right { get; set; }
        public GameCellType Type
        {
            get
            {
                return Left.Type;
            }
            set
            {
                Right.Type = value;
                Left.Type = value;
            }
        }

        public GameBlock(int row, int col)
        {
            Left = new GameCell(row, col, GameCellType.None, GameCellColor.None);
            Right = new GameCell(row, col + 1, GameCellType.None, GameCellColor.None);

            Left.BindCell = Right;
            Right.BindCell = Left;
        }

        public GameBlock(IGameBlock nextBlock)
        {
            Left = new GameCell(0, 3, GameCellType.Block, nextBlock.Left.Color);
            Right = new GameCell(0, 4, GameCellType.Block, nextBlock.Right.Color);

            Left.BindCell = Right;
            Right.BindCell = Left;
        }

        public void SetRandomColor()
        {
            Left.Color = GameColorFactory.CreateRandomColor();
            Right.Color = GameColorFactory.CreateRandomColor();
        }

        public bool CanFallDown(IGameField field) => Left.CanFallDown(field) && Right.CanFallDown(field);

        public void FallDown(IGameField field)
        {
            Left.FallDown();
            Right.FallDown();
        }

        public bool CanTurn(IGameField field)
        {
            int state = GetState();

            switch (state)
            {
                case 1: return Left.Row > 0 && field[Left.Row - 1, Left.Column + 1].Type == GameCellType.None;
                case 2: return Left.Column > 0 && field[Left.Row + 1, Left.Column - 1].Type == GameCellType.None;
                case 3: return Right.Row > 0 && field[Right.Row - 1, Right.Column + 1].Type == GameCellType.None;
                case 4: return Right.Column > 0 && field[Right.Row + 1, Right.Column - 1].Type == GameCellType.None;
                default: return false;
            }
        }

        public void Turn()
        {
            int state = GetState();

            switch (state)
            {
                case 1: Left.Row--; Left.Column++; break;
                case 2: Left.Row++; Right.Column--; break;
                case 3: Right.Row--; Right.Column++; break;
                case 4: Right.Row++; Left.Column--; break;
            }
        }

        public bool CanMove(GameSide side, IGameField field)
        {
            var state = GetState();

            switch (side)
            {
                case GameSide.Left:
                    switch (state)
                    {
                        case 1: return Left.Column > 0 && field[Left.Row, Left.Column - 1].Type == GameCellType.None;
                        case 2:
                            if (Left.Column == 0 && Right.Column == 0) return false;
                            return field[Left.Row, Left.Column - 1].Type == GameCellType.None
                                && field[Right.Row, Right.Column - 1].Type == GameCellType.None;

                        case 3: return Right.Column > 0 && field[Right.Row, Right.Column - 1].Type == GameCellType.None;
                        case 4:
                            if (Left.Column == 0 && Right.Column == 0) return false;
                            return field[Left.Row, Left.Column - 1].Type == GameCellType.None
                                && field[Right.Row, Right.Column - 1].Type == GameCellType.None;
                    }
                    break;
                case GameSide.Right:
                    switch (state)
                    {
                        case 1: return Right.Column + 1 < field.Width && field[Right.Row, Right.Column + 1].Type == GameCellType.None;
                        case 2:
                            if (Left.Column + 1 == field.Width && Right.Column + 1 == field.Width) return false;
                            return field[Left.Row, Left.Column + 1].Type == GameCellType.None
                                && field[Right.Row, Right.Column + 1].Type == GameCellType.None;

                        case 3: return Left.Column + 1 < field.Width && field[Left.Row, Left.Column + 1].Type == GameCellType.None;
                        case 4:
                            if (Left.Column + 1 == field.Width && Right.Column + 1 == field.Width) return false;
                            return field[Left.Row, Left.Column + 1].Type == GameCellType.None
                                && field[Right.Row, Right.Column + 1].Type == GameCellType.None;

                    }
                    break;
                case GameSide.Down:
                    return Left.CanFallDown(field) && Right.CanFallDown(field);
            }

            throw new Exception("Что-то пошло не так");
        }

        public void Move(GameSide side)
        {
            if (side == GameSide.Left)
            {
                Left.Column--;
                Right.Column--;
            }

            if (side == GameSide.Right)
            {
                Left.Column++;
                Right.Column++;
            }

            if (side == GameSide.Down)
            {
                Left.Row++;
                Right.Row++;
            }
        }

        /// <summary>
        /// Возвращает состояние летящего блока (позицию)
        /// 1      2      3      4
        /// N N -> N L -> N N -> N R
        /// L R    N R    R L    N L
        /// </summary>
        /// <returns></returns>
        private int GetState()
        {
            if (Left.Row == Right.Row) return Left.Column < Right.Column ? 1 : 3;
            return Left.Row < Right.Row ? 2 : 4;
        }
    }
}
