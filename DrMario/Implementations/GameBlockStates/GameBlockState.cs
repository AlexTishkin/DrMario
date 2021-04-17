using DrMario.Interfaces;

namespace DrMario.Implementations.GameBlockStates
{
    /// <summary>
    /// Состояние летящего блока
    /// (Позиции левого и правого блока)
    /// </summary>
    public abstract class GameBlockState
    {
        protected IGameBlock _gameBlock;

        public void SetContext(IGameBlock gameBlock)
        {
            _gameBlock = gameBlock;
        }

        public bool CanFallDown(IGameField field) => _gameBlock.Left.CanFallDown(field) && _gameBlock.Right.CanFallDown(field);

        public void FallDown()
        {
            _gameBlock.Left.FallDown();
            _gameBlock.Right.FallDown();
        }

        public void Move(GameSide side)
        {
            if (side == GameSide.Left)
            {
                _gameBlock.Left.Column--;
                _gameBlock.Right.Column--;
            }

            if (side == GameSide.Right)
            {
                _gameBlock.Left.Column++;
                _gameBlock.Right.Column++;
            }

            if (side == GameSide.Down)
            {
                _gameBlock.Left.Row++;
                _gameBlock.Right.Row++;
            }
        }

        public abstract bool CanTurn(IGameField field);

        public abstract void Turn();

        public abstract bool CanMove(GameSide side, IGameField field);
    }
}
