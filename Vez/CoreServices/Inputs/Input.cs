using System.Runtime.CompilerServices;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

//using Vez.CoreServices.Inputs.Virtual;
using Vez.Maths;
using Vez.Utils;
using Vez.Utils.Collections;

namespace Vez.CoreServices.Inputs
{
    public class Input : IUpdateService
    {
        public Emitter<InputEventType, InputEvent> Emitter;

        public GamePadData[] GamePads;
        public const float DEFAULT_DEADZONE = 0.1f;

        internal Vector2 _resolutionScale = new(1);
        internal Point _resolutionOffset;
        KeyboardState _previousKbState;
        KeyboardState _currentKbState;
        MouseState _previousMouseState;
        MouseState _currentMouseState;
        //internal FastList<VirtualInput> _virtualInputs = new FastList<VirtualInput>();
        int _maxSupportedGamePads;

        private IInputStateProvider _inputStateService;

        /// <summary>
        /// the TouchInput details when on a device that supports touch
        /// </summary>
        //public TouchInput Touch;

        /// <summary>
        /// set by the Scene and used to scale mouse input for cases where the Scene render target is a different size
        /// than the backbuffer. This situation basically results in mouse coordinates in screen space instead of
        /// in the render target coordinate system;
        /// </summary>
        public Vector2 ResolutionScale
        {
            get => _resolutionScale;
            set => _resolutionScale = value;
        }

        /// <summary>
        /// set by the Scene and used to get mouse input from raw screen coordinates to render target coordinates. Any
        /// SceneResolutionPolicy that can result in letterboxing could potentially have an offset (basically, the
        /// letterbox portion of the render).
        /// </summary>
        /// <returns></returns>
        public Vector2 ResolutionOffset
        {
            get => _resolutionOffset.ToVector2();
            set => _resolutionOffset = value.ToPoint();
        }

        /// <summary>
        /// gets/sets the maximum supported gamepads
        /// </summary>
        /// <value></value>
        public int MaxSupportedGamePads
        {
            get { return _maxSupportedGamePads; }
            set
            {
#if FNA
				_maxSupportedGamePads = Mathf.Clamp( value, 1, 8 );
#else
                _maxSupportedGamePads = Mathf.Clamp(value, 1, GamePad.MaximumGamePadCount);
#endif
                GamePads = new GamePadData[_maxSupportedGamePads];
                for (var i = 0; i < _maxSupportedGamePads; i++)
                    GamePads[i] = new GamePadData((PlayerIndex)i, this);
            }
        }


        public Input(IInputStateProvider inputStateService)
        {
            Emitter = new Emitter<InputEventType, InputEvent>();
            _inputStateService = inputStateService;
            //Touch = new TouchInput(this);

            _previousKbState = new KeyboardState();
            _currentKbState = inputStateService.GetKeyboardState();

            _previousMouseState = new MouseState();
            _currentMouseState = inputStateService.GetMouseState();

            MaxSupportedGamePads = 1;
        }


        public void Update(float deltaTime)
        {
            //Touch.Update();

            _previousKbState = _currentKbState;
            _currentKbState = _inputStateService.GetKeyboardState();

            _previousMouseState = _currentMouseState;
            _currentMouseState = _inputStateService.GetMouseState();

            for (var i = 0; i < _maxSupportedGamePads; i++)
                GamePads[i].Update(deltaTime);

            //for (var i = 0; i < _virtualInputs.Length; i++)
            //    _virtualInputs.Buffer[i].Update();
        }

        /// <summary>
        /// this takes into account the SceneResolutionPolicy and returns the value scaled to the RenderTargets coordinates
        /// </summary>
        /// <value>The scaled position.</value>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 ScaledPosition(Vector2 position)
        {
            var scaledPos = new Vector2(position.X - _resolutionOffset.X, position.Y - _resolutionOffset.Y);
            return scaledPos * _resolutionScale;
        }

        /// <summary>
        /// to be used with great care! This lets you override the current MouseState. This is useful
        /// when the Nez render is embedded in a larger window so that mouse coordinates can be translated
        /// to Nez space from the outer window coordinates and for simulating mouse input.
        /// </summary>
        /// <param name="state"></param>
        public void SetCurrentMouseState(MouseState state)
        {
            _currentMouseState = state;
        }

        /// <summary>
        /// useful for simulating mouse input
        /// </summary>
        /// <param name="state"></param>
        public void SetPreviousMouseState(MouseState state)
        {
            _previousMouseState = state;
        }

        /// <summary>
        /// useful for simulating keyboard input
        /// </summary>
        /// <param name="state">State.</param>
        public void SetCurrentKeyboardState(KeyboardState state)
        {
            _currentKbState = state;
        }

        /// <summary>
        /// useful for simulating keyboard input
        /// </summary>
        /// <param name="state">State.</param>
        public void SetPreviousKeyboardState(KeyboardState state)
        {
            _previousKbState = state;
        }

        #region Keyboard

        /// <summary>
        /// returns the previous KeyboardState from the last frame
        /// </summary>
        /// <value></value>
        public KeyboardState PreviousKeyboardState => _previousKbState;

        /// <summary>
        /// returns the KeyboardState from this frame
        /// </summary>
        /// <value></value>
        public KeyboardState CurrentKeyboardState => _currentKbState;


        /// <summary>
        /// only true if down this frame and not down the previous frame
        /// </summary>
        /// <returns><c>true</c>, if key pressed was gotten, <c>false</c> otherwise.</returns>
        public bool IsKeyPressed(Keys key)
        {
            return _currentKbState.IsKeyDown(key) && !_previousKbState.IsKeyDown(key);
        }


        /// <summary>
        /// true the entire time the key is down
        /// </summary>
        /// <returns><c>true</c>, if key down was gotten, <c>false</c> otherwise.</returns>
        public bool IsKeyDown(Keys key)
        {
            return _currentKbState.IsKeyDown(key);
        }


        /// <summary>
        /// true only the frame the key is released
        /// </summary>
        /// <returns><c>true</c>, if key up was gotten, <c>false</c> otherwise.</returns>
        public bool IsKeyReleased(Keys key)
        {
            return !_currentKbState.IsKeyDown(key) && _previousKbState.IsKeyDown(key);
        }


        /// <summary>
        /// only true if one of the keys is down this frame
        /// </summary>
        /// <returns><c>true</c>, if key pressed was gotten, <c>false</c> otherwise.</returns>
        public bool IsKeyPressed(Keys keyA, Keys keyB)
        {
            return IsKeyPressed(keyA) || IsKeyPressed(keyB);
        }


        /// <summary>
        /// true while either of the keys are down
        /// </summary>
        /// <returns><c>true</c>, if key down was gotten, <c>false</c> otherwise.</returns>
        public bool IsKeyDown(Keys keyA, Keys keyB)
        {
            return IsKeyDown(keyA) || IsKeyDown(keyB);
        }


        /// <summary>
        /// true only the frame one of the keys are released
        /// </summary>
        /// <returns><c>true</c>, if key up was gotten, <c>false</c> otherwise.</returns>
        public bool IsKeyReleased(Keys keyA, Keys keyB)
        {
            return IsKeyReleased(keyA) || IsKeyReleased(keyB);
        }

        #endregion


        #region Mouse

        /// <summary>
        /// returns the previous mouse state. Use with caution as it only contains raw data and does not take camera scaling into affect like
        /// Input.mousePosition does.
        /// </summary>
        /// <value>The state of the previous mouse.</value>
        public MouseState PreviousMouseState => _previousMouseState;

        /// <summary>
        /// returns the current mouse state. Use with caution as it only contains raw data and does not take camera scaling into affect like
        /// Input.mousePosition does.
        /// </summary>
        public MouseState CurrentMouseState => _currentMouseState;

        /// <summary>
        /// only true if down this frame
        /// </summary>
        public bool LeftMouseButtonPressed =>
            _currentMouseState.LeftButton == ButtonState.Pressed &&
            _previousMouseState.LeftButton == ButtonState.Released;

        /// <summary>
        /// true while the button is down
        /// </summary>
        public bool LeftMouseButtonDown => _currentMouseState.LeftButton == ButtonState.Pressed;

        /// <summary>
        /// true only the frame the button is released
        /// </summary>
        public bool LeftMouseButtonReleased =>
            _currentMouseState.LeftButton == ButtonState.Released &&
            _previousMouseState.LeftButton == ButtonState.Pressed;

        /// <summary>
        /// only true if pressed this frame
        /// </summary>
        public bool RightMouseButtonPressed =>
            _currentMouseState.RightButton == ButtonState.Pressed &&
            _previousMouseState.RightButton == ButtonState.Released;

        /// <summary>
        /// true while the button is down
        /// </summary>
        public bool RightMouseButtonDown => _currentMouseState.RightButton == ButtonState.Pressed;

        /// <summary>
        /// true only the frame the button is released
        /// </summary>
        public bool RightMouseButtonReleased =>
            _currentMouseState.RightButton == ButtonState.Released &&
            _previousMouseState.RightButton == ButtonState.Pressed;

        /// <summary>
        /// only true if down this frame
        /// </summary>
        public bool MiddleMouseButtonPressed =>
            _currentMouseState.MiddleButton == ButtonState.Pressed &&
            _previousMouseState.MiddleButton == ButtonState.Released;

        /// <summary>
        /// true while the button is down
        /// </summary>
        public bool MiddleMouseButtonDown => _currentMouseState.MiddleButton == ButtonState.Pressed;

        /// <summary>
        /// true only the frame the button is released
        /// </summary>
        public bool MiddleMouseButtonReleased =>
            _currentMouseState.MiddleButton == ButtonState.Released &&
            _previousMouseState.MiddleButton == ButtonState.Pressed;

        /// <summary>
        /// only true if down this frame
        /// </summary>
        public bool FirstExtendedMouseButtonPressed =>
            _currentMouseState.XButton1 == ButtonState.Pressed &&
            _previousMouseState.XButton1 == ButtonState.Released;

        /// <summary>
        /// true while the button is down
        /// </summary>
        public bool FirstExtendedMouseButtonDown => _currentMouseState.XButton1 == ButtonState.Pressed;

        /// <summary>
        /// true only the frame the button is released
        /// </summary>
        public bool FirstExtendedMouseButtonReleased =>
            _currentMouseState.XButton1 == ButtonState.Released &&
            _previousMouseState.XButton1 == ButtonState.Pressed;

        /// <summary>
        /// only true if down this frame
        /// </summary>
        public bool SecondExtendedMouseButtonPressed =>
            _currentMouseState.XButton2 == ButtonState.Pressed &&
            _previousMouseState.XButton2 == ButtonState.Released;

        /// <summary>
        /// true while the button is down
        /// </summary>
        public bool SecondExtendedMouseButtonDown => _currentMouseState.XButton2 == ButtonState.Pressed;

        /// <summary>
        /// true only the frame the button is released
        /// </summary>
        public bool SecondExtendedMouseButtonReleased =>
            _currentMouseState.XButton2 == ButtonState.Released &&
            _previousMouseState.XButton2 == ButtonState.Pressed;

        /// <summary>
        /// gets the raw ScrollWheelValue
        /// </summary>
        /// <value>The mouse wheel.</value>
        public int MouseWheel => _currentMouseState.ScrollWheelValue;

        /// <summary>
        /// gets the delta ScrollWheelValue
        /// </summary>
        /// <value>The mouse wheel delta.</value>
        public int MouseWheelDelta => _currentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue;

        /// <summary>
        /// unscaled mouse position. This is the actual screen space value
        /// </summary>
        /// <value>The raw mouse position.</value>
        public Point RawMousePosition => new Point(_currentMouseState.X, _currentMouseState.Y);

        /// <summary>
        /// alias for scaledMousePosition
        /// </summary>
        /// <value>The mouse position.</value>
        public Vector2 MousePosition => ScaledMousePosition;

        /// <summary>
        /// this takes into account the SceneResolutionPolicy and returns the value scaled to the RenderTargets coordinates
        /// </summary>
        /// <value>The scaled mouse position.</value>
        public Vector2 ScaledMousePosition => ScaledPosition(new Vector2(_currentMouseState.X, _currentMouseState.Y));

        public Point MousePositionDelta =>
            new Point(_currentMouseState.X, _currentMouseState.Y) -
            new Point(_previousMouseState.X, _previousMouseState.Y);

        public Vector2 ScaledMousePositionDelta
        {
            get
            {
                var pastPos = new Vector2(_previousMouseState.X - _resolutionOffset.X,
                    _previousMouseState.Y - _resolutionOffset.Y);
                pastPos *= _resolutionScale;
                return ScaledMousePosition - pastPos;
            }
        }

        #endregion
    }


    public enum InputEventType
    {
        GamePadConnected,
        GamePadDisconnected
    }


    public struct InputEvent
    {
        public int GamePadIndex;
    }


    /// <summary>
    /// comparer that should be passed to a dictionary constructor to avoid boxing/unboxing when using an enum as a key
    /// on Mono
    /// </summary>
    public struct InputEventTypeComparer : IEqualityComparer<InputEventType>
    {
        public bool Equals(InputEventType x, InputEventType y)
        {
            return x == y;
        }


        public int GetHashCode(InputEventType obj)
        {
            return (int)obj;
        }
    }
}
