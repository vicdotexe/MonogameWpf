using Microsoft.Xna.Framework.Input;

namespace Vez.CoreServices.Inputs
{
    public interface IInputStateProvider
    {
        MouseState GetMouseState();
        KeyboardState GetKeyboardState();
    }

    public class InputStateProvider : IInputStateProvider
    {
        public MouseState GetMouseState() => Mouse.GetState();

        public KeyboardState GetKeyboardState() => Keyboard.GetState();

    }
}