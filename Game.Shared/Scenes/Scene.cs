using System.Drawing;

using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;

using Game.Shared.Systems;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Vez.CoreServices;
using Vez.Ecs;
using Vez.Ecs.Children;
using Vez.Graphics;
using Vez.Graphics.Textures;
using Vez.Utils.Extensions;

using Color = Microsoft.Xna.Framework.Color;

namespace Game.Shared.Scenes
{
    public class TestScene : IUpdateService, IDrawService
    {
        private World World { get; }
        private ISystem<Time> _updateSystems;
        private ISystem<Batcher> _drawSystems;
        private Batcher _spriteBatch;
        private World _currentWorld;
        private Sprite _pixel;

        private Time _time;

        public TestScene(Time time, SystemFactory factory, WorldService worldService, Batcher spriteBatch, GraphicsDevice graphicsDevice)
        {
            _time = time;
            _spriteBatch = spriteBatch;

            _updateSystems = new SequentialSystem<Time>(
                factory.New<MousePositionSystem>(),
                factory.New<HierarchyTransformSystem>()
            );


            _drawSystems = new SequentialSystem<Batcher>(
                factory.New<PreDrawSystem>(),
                factory.New<SpriteRenderSystem>(),
                factory.New<DebugDrawSystem>(),
                factory.New<PostDrawSystem>()
            );

            World = worldService.CurrentWorld;
            _pixel = spriteBatch.PixelTexture();


            var entity = worldService.CurrentWorld.CreateEntity();
            entity.Set(new SpriteComponent(_pixel){Color = Color.Purple});
            entity.Set(new Transform { Position = new Vector2(100, 100), Scale = new (50)});
            entity.Set(new TrackMouseLocation());

            var child = worldService.CurrentWorld.CreateEntity();
            child.Set(new Parent(entity));
            child.Set(new Transform());
            child.Set(new LocalTransform { Position = new Vector2(25, 25), Scale = new(0.5f) });
            child.Set(new SpriteComponent(_pixel) { Color = Color.Green });

        }

        public void Update(float deltaTime)
        {
            _updateSystems.Update(_time);
        }

        public void Draw()
        {
            _drawSystems.Update(_spriteBatch);
        }
    }

    public class HierarchyTransformSystem : BaseSystem<Time>
    {
        private EntitySet _entitySet;
        public HierarchyTransformSystem(WorldService worldService) : base()
        {
            _entitySet = worldService.CurrentWorld.GetEntities().With<Transform>().With<Parent>().AsSet();

        }

        public override void Update(Time state)
        {
            foreach (var entity in _entitySet.GetEntities())
            {
                LocalTransform localTransform = entity.Get<LocalTransform>();

                Transform parentTransform = entity.Get<Parent>().Value.Get<Transform>();
                // Function to rotate a point around the origin by a specific angle
                Vector2 Rotate(Vector2 point, float angle)
                {
                    float cos = MathF.Cos(angle);
                    float sin = MathF.Sin(angle);
                    return new Vector2(
                        point.X * cos - point.Y * sin,
                        point.X * sin + point.Y * cos
                    );
                }

                // Rotating the entity's local position by the parent's rotation angle
                Vector2 rotatedLocalPosition = Rotate(localTransform.Position, parentTransform.Rotation);


                entity.Set<Transform>(parentTransform with
                {
                    Position = parentTransform.Position + rotatedLocalPosition,
                    Scale = parentTransform.Scale * localTransform.Scale,
                    Rotation = entity.Get<Transform>().Rotation + 0.01f
                });
            }

        }
    }
    
    public class DebugDrawSystem : BaseSystem<Batcher>
    {
        private EntitySet _entitySet;
        public DebugDrawSystem(WorldService worldService) : base()
        {
            _entitySet = worldService.CurrentWorld.GetEntities().With<Transform>().With<SpriteComponent>().AsSet();
        }

        public override void Update(Batcher state)
        {
            foreach (var entity in _entitySet.GetEntities())
            {
                Transform transform = entity.Get<Transform>();
                SpriteComponent spriteComponent = entity.Get<SpriteComponent>();
                
                entity.Set<Transform>(transform with
                {
                    Rotation = transform.Rotation + 0.01f
                });

                Vector2[] points = new Vector2[4];

                // Getting the sprite's source rectangle dimensions and applying scale
                float width = spriteComponent.SourceRectangle.Width * transform.Scale.X;
                float height = spriteComponent.SourceRectangle.Height * transform.Scale.Y;

                // Calculating the corners of the sprite in local coordinates
                Vector2 topLeft = new Vector2(-width / 2, -height / 2);
                Vector2 topRight = new Vector2(width / 2, -height / 2);
                Vector2 bottomRight = new Vector2(width / 2, height / 2);
                Vector2 bottomLeft = new Vector2(-width / 2, height / 2);

                // Rotating the points based on the transform's rotation
                Vector2 Rotate(Vector2 point, float angle)
                {
                    float cos = MathF.Cos(angle);
                    float sin = MathF.Sin(angle);
                    return new Vector2(
                        point.X * cos - point.Y * sin,
                        point.X * sin + point.Y * cos
                    );
                }

                // Applying the rotation to each corner (without applying the translation)
                points[0] = Rotate(topLeft, transform.Rotation);
                points[1] = Rotate(topRight, transform.Rotation);
                points[2] = Rotate(bottomRight, transform.Rotation);
                points[3] = Rotate(bottomLeft, transform.Rotation);

                // Now, you can draw the polygon relative to the transform's position
                state.DrawPolygon(transform.Position, points, Color.Yellow);

                // Finding the AABB that encompasses the rotated sprite
                float minX = float.MaxValue;
                float minY = float.MaxValue;
                float maxX = float.MinValue;
                float maxY = float.MinValue;

                foreach (Vector2 point in points)
                {
                    minX = MathF.Min(minX, point.X + transform.Position.X);
                    minY = MathF.Min(minY, point.Y + transform.Position.Y);
                    maxX = MathF.Max(maxX, point.X + transform.Position.X);
                    maxY = MathF.Max(maxY, point.Y + transform.Position.Y);
                }


                // Drawing the AABB
                Vector2[] aabbPoints = new Vector2[4]
                {
                    new Vector2(minX, minY),
                    new Vector2(maxX, minY),
                    new Vector2(maxX, maxY),
                    new Vector2(minX, maxY)
                };
                // Location is the position of the top-left corner of the AABB
                Vector2 location = new Vector2(minX, minY);

                // Size is calculated as the difference between the maximum and minimum values of x and y
                Vector2 size = new Vector2(maxX - minX, maxY - minY);

                // Now you can use the location and size for drawing or other purposes
                // For example, using a hypothetical DrawRectangle method
                state.DrawHollowRect(location, size.X, size.Y, Color.Blue);
            }
            
        }
    }
}