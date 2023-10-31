using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Monogame.Wpf;

using Vez;
using Vez.CoreServices;
using Vez.CoreServices.Inputs;

namespace MgEditor
{
    public class GameViewModel : MonoGameViewModel, IGame
    {
        private Core? _core;
        private readonly IServiceProvider _services;
        WpfInputService? _inputService;

        public GameViewModel(IServiceProvider services) : base()
        {
            _services = services;
            
        }

        public override void Initialize(WpfMouse mouse)
        {
            _inputService = _services.GetRequiredService<IInputStateProvider>() as WpfInputService;
            _inputService?.Initialize(mouse);

            _core = _services.GetRequiredService<Core>();
            _core.Initialize();
        }

        public override void LoadContent()
        {
            _core?.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _core?.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _core?.Draw(gameTime);
        }

        public void Run()
        {

        }
    }
}