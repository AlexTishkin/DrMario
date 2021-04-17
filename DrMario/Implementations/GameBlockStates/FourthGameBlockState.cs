using DrMario.Interfaces;
using System;

namespace DrMario.Implementations.GameBlockStates
{
    /// <summary>
    /// 4 Состояние летящего блока
    /// N R
    /// N L
    /// </summary>
    public class FourthGameBlockState : GameBlockState
    {
        public override bool CanTurn(IGameField field)
        {
            return _gameBlock.Right.Column > 0 && field[_gameBlock.Right.Row + 1, _gameBlock.Right.Column - 1].Type == GameCellType.None;
        }

        public override void Turn()
        {
            _gameBlock.Right.Row++;
            _gameBlock.Left.Column--;
            _gameBlock.TransitionTo(new FirstGameBlockState());
        }

        public override bool CanMove(GameSide side, IGameField field)
        {
            switch (side)
            {
                case GameSide.Left:
                    if (_gameBlock.Left.Column == 0 && _gameBlock.Right.Column == 0) return false;
                    return field[_gameBlock.Left.Row, _gameBlock.Left.Column - 1].Type == GameCellType.None
                        && field[_gameBlock.Right.Row, _gameBlock.Right.Column - 1].Type == GameCellType.None;

                case GameSide.Right:
                    if (_gameBlock.Left.Column + 1 == field.Width && _gameBlock.Right.Column + 1 == field.Width) return false;
                    return field[_gameBlock.Left.Row, _gameBlock.Left.Column + 1].Type == GameCellType.None
                        && field[_gameBlock.Right.Row, _gameBlock.Right.Column + 1].Type == GameCellType.None;

                case GameSide.Down:
                    return _gameBlock.Left.CanFallDown(field) && _gameBlock.Right.CanFallDown(field);

                default: throw new ArgumentException("Неверно указана сторона side (GameSide)");
            }
        }
    }
}
