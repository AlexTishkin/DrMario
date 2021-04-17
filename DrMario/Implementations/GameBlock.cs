using DrMario.Implementations.GameBlockStates;
using DrMario.Interfaces;

namespace DrMario.Implementations
{
    public class GameBlock : IGameBlock
    {
        public IGameCell Left { get; set; }
        public IGameCell Right { get; set; }
        public GameCellType Type
        {
            get => Left.Type;
            set => Right.Type = Left.Type = value;
        }

        // Ссылка на текущее состояние Контекста
        private GameBlockState _state = null;

        public GameBlock(int row, int col)
        {
            Left = new GameCell(row, col, GameCellType.None, GameCellColor.None);
            Right = new GameCell(row, col + 1, GameCellType.None, GameCellColor.None);

            Left.BindCell = Right;
            Right.BindCell = Left;
            TransitionTo(new FirstGameBlockState());
        }

        public GameBlock(IGameBlock nextBlock)
        {
            Left = new GameCell(0, 3, GameCellType.Block, nextBlock.Left.Color);
            Right = new GameCell(0, 4, GameCellType.Block, nextBlock.Right.Color);

            Left.BindCell = Right;
            Right.BindCell = Left;
            TransitionTo(new FirstGameBlockState());
        }

        public void SetRandomColor()
        {
            Left.Color = GameColorFactory.CreateRandomColor();
            Right.Color = GameColorFactory.CreateRandomColor();
        }

        // Контекст делегирует часть своего поведения текущему объекту Состояния
        public void TransitionTo(GameBlockState state)
        {
            _state = state;
            _state.SetContext(this);
        }

        public bool CanFallDown(IGameField field) => _state.CanFallDown(field);

        public void FallDown() => _state.FallDown();

        public bool CanTurn(IGameField field) => _state.CanTurn(field);

        public void Turn() => _state.Turn();

        public bool CanMove(GameSide side, IGameField field) => _state.CanMove(side, field);

        public void Move(GameSide side) => _state.Move(side);

    }
}
