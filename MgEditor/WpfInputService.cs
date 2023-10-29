using Microsoft.Xna.Framework.Input;

using Monogame.Wpf;

using Vez.CoreServices.Inputs;

namespace MgEditor
{
    public class WpfInputService : IInputStateProvider
    {
        private WpfMouse _mouse;


        public void Initialize(WpfMouse mouse)
        {
            _mouse = mouse;
        }

        public MouseState GetMouseState() => _mouse.GetState();

        public KeyboardState GetKeyboardState() => new ();
    }

}