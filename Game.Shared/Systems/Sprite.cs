using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Shared.Systems
{
    public class SpriteComponent
    {
        public SpriteComponent(Texture2D texture)
        {
            Texture = texture;
        }

        public Texture2D Texture { get; set; }
        public Vector2 Origin { get; set; }
        public Color Color { get; set; } = Color.White;
        public SpriteEffects SpriteEffects { get; set; }
        public Rectangle? SourceRectangle { get; set; }

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