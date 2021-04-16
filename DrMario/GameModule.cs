using Autofac;
using DrMario.Implementations;
using DrMario.Interfaces;

namespace DrMario
{
    public class GameModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GameForm>();
            builder.RegisterType<Game>().As<IGame>();
        }
    }
}
