using Microsoft.Xna.Framework.Graphics;

namespace Vez.CoreServices
{
    public class Graphics
    {
        public SpriteBatch SpriteBatch { get; set; }

        public Graphics(IGraphicsDeviceService graphicsDeviceService)
        {
            SpriteBatch = new SpriteBatch(graphicsDeviceService.GraphicsDevice);
        }
    }
}