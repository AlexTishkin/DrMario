using Autofac;
using DrMario.Display;
using DrMario.Implementations;
using DrMario.Interfaces;
using System.Windows.Forms;

namespace DrMario
{
    public partial class GameForm : Form
    {
        private IContainer _container;
        private IGame _game;
        private IDisplay _gameDisplay;

        public GameForm()
        {
            InitializeComponent();
            GameModuleInitialize();
            GameInitialize();
        }

        /// <summary>
        /// Инверсия зависимостей (AutoFac)
        /// </summary>
        private void GameModuleInitialize()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<GameModule>();
            _container = builder.Build();
        }

        /// <summary>
        /// Инициализация игровых настроек
        /// </summary>
        public void GameInitialize()
        {
            _game = _container.Resolve<IGame>();
            _gameDisplay = new Display.Display(_game, gamePictureBox);

            // Управление (Клавиатура)
            KeyDown += (sender, e) => _game.OnUserAction(KeyToGameKeyConvert.ToGameKey(e.KeyCode));

            // Такт игры (Падение блока, обвал блока и т.д)
            blockTimer.Tick += (sender, e) => _game.GameTact();
            blockTimer.Interval = 500;

            // Отрисовка на основные события игры
            KeyDown += (sender, e) => _gameDisplay.ShowGameState();
            blockTimer.Tick += (sender, e) => _gameDisplay.ShowGameState();
            blockTimer.Start();
        }
    }
}
