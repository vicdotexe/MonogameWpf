using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vez.CoreServices
{
    public interface IGame : IDisposable
    {
        IGraphicsDeviceService GraphicsDeviceService { get; }
        void Run();

    }
}