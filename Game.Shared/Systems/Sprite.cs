using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Vez.Graphics.Textures;

namespace Game.Shared.Systems
{
    public class SpriteComponent
    {
        public SpriteComponent(Sprite sprite)
        {
            Sprite = sprite;
        }

        public Sprite Sprite { get; set; }
        public Color Color { get; set; } = Color.White;
        public SpriteEffects SpriteEffects { get; set; }
        public Rectangle SourceRectangle { get; set; } = new Rectangle(0, 0, 1, 1);

    }


    public record LocalTransform()
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; } 
        public Vector2 Scale { get; set; } = Vector2.One;
    }

    public record Transform()
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
    }

    public class TrackMouseLocation
    {

    }
}