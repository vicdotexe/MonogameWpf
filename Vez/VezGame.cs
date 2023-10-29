using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Vez.CoreServices;

namespace Vez
{
    public class VezGame : Game, IGame
    {
        private Core? _core;

        public IGraphicsDeviceService GraphicsDeviceService { get; }

        private readonly IServiceProvider _services;

        public VezGame(IServiceProvider services) : base()
        {
            GraphicsDeviceService = new GraphicsDeviceManager(this);
            _services = services;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _core = _services.GetRequiredService<Core>();
            _core?.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _core?.LoadContent();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            _core?.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _core?.Draw(gameTime);
        }
    }
}