using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Vez.Graphics
{
    public class SpriteEffect : Effect
    {
        EffectParameter _matrixTransformParam;


        public SpriteEffect(GraphicsDevice graphicsDevice, byte[] effectByteCode) : base(graphicsDevice, effectByteCode)
        {
            _matrixTransformParam = Parameters["MatrixTransform"];
        }


        public void SetMatrixTransform(ref Matrix matrixTransform)
        {
            _matrixTransformParam.SetValue(matrixTransform);
        }
    }
}