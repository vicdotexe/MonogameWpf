using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Vez.CoreServices;
using Microsoft.Extensions.Configuration;

namespace Vez
{
    public class VezGame : Game, IGame
    {
        private Core? _core;
        public IGraphicsDeviceService GraphicsDeviceService { get; }
        private readonly IServiceProvider _services;

        public VezGame(IServiceProvider services)
        {
            GraphicsDeviceService = new GraphicsDeviceManager(this);
            _services = services;
        }

        protected override void Initialize()
        {
            _core = _services.GetRequiredService<Core>();
            _core?.Initialize();
        }

        protected override void LoadContent() => _core?.LoadContent();

        protected override void Update(GameTime gameTime) => _core?.Update(gameTime);
        
        protected override void Draw(GameTime gameTime) => _core?.Draw(gameTime);
    }


    public class GameFactory
    {
        public TGame CreateGame<TGame>(Action<CoreConfiguration, ServiceCollection> configuration) where TGame : class, IGame
        {
            var services = new ServiceCollection();
            services.AddSingleton<TGame>();
            services.AddSingleton<Core>();
            var builder = new CoreConfiguration(services);
            configuration(builder, services);

            services.AddSingleton(isp => isp.GetRequiredService<TGame>().GraphicsDeviceService.GraphicsDevice);
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<TGame>();
        }

        public TGame CreateGame<TGame,TConfig>(Action<CoreConfiguration, ServiceCollection>? configuration = null) where TGame : class, IGame where TConfig : GameConfiguration
        {
            TConfig gameConfig = (TConfig)Activator.CreateInstance(typeof(TConfig), configuration);
            return gameConfig.CreateGame<TGame>();
        }
    }
}
