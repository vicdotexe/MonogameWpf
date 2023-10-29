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

namespace Game.Shared.Scenes
{
    public class TestScene : IUpdateService, IDrawService
    {
        private World World { get; }
        private ISystem<Time> _updateSystems;
        private ISystem<Batcher> _drawSystems;
        private Batcher _spriteBatch;

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
                factory.New<PostDrawSystem>()
            );

            var texture2d = new Texture2D(graphicsDevice, 1, 1);
            texture2d.SetData(new[] { Color.White });


            var entity = worldService.CurrentWorld.CreateEntity();
            entity.Set(new SpriteComponent(texture2d){Color = Color.Purple});
            entity.Set(new Transform { Position = new Vector2(100, 100), Scale = new (50)});
            entity.Set(new TrackMouseLocation());

            var child = worldService.CurrentWorld.CreateEntity();
            child.Set(new Parent(entity));
            child.Set(new Transform());
            child.Set(new LocalTransform { Position = new Vector2(25, 25), Scale = new(0.5f) });
            child.Set(new SpriteComponent(texture2d) { Color = Color.Green });

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


                entity.Set<Transform>(parentTransform with
                {
                    Position = parentTransform.Position + localTransform.Position,
                    Scale = parentTransform.Scale * localTransform.Scale
                });
            }

        }
    }
}