using DefaultEcs;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Vez.CoreServices;
using Vez.CoreServices.Inputs;
using Vez.Ecs;
using Vez.Graphics;
using Vez.Utils.Extensions;

namespace Game.Shared.Systems
{
    public class SpriteRenderSystem : BaseSystem<Batcher>
    {
        WorldService _worldService;
        EntitySet _entitySet;
        Input _input;

        public SpriteRenderSystem(WorldService worldService, Input input)
        {
            _worldService = worldService;
            _entitySet = worldService.CurrentWorld.GetEntities().With<Transform>().With<SpriteComponent>().AsSet();
            _input = input;
        }

        public override void Update(Batcher state)
        {
            if (_worldService is not { CurrentWorld: { } world })
                return;

            foreach (var entity in _entitySet.GetEntities())
            {
                var transform = entity.Get<Transform>();
                var sprite = entity.Get<SpriteComponent>();

                state.Draw(sprite.Sprite, transform.Position, sprite.SourceRectangle, sprite.Color, transform.Rotation, sprite.Sprite.Origin, transform.Scale, SpriteEffects.None, 0);
            }

            state.DrawLine(new(), _input.MousePosition, Color.Red);
        }
    }

    public class MousePositionSystem : BaseSystem<Time>
    {
        private readonly Input _input;
        private readonly EntitySet _entitySet;
        private readonly WorldService _worldService;

        public MousePositionSystem(WorldService worldService, Input input)
        {
            _worldService = worldService;
            _input = input;
            _entitySet = worldService.CurrentWorld.GetEntities().With<Transform>().With<TrackMouseLocation>().AsSet();
        }

        public override void Update(Time state)
        {
            if (_worldService is not { CurrentWorld: { } world })
                return;
            foreach (var entity in _entitySet.GetEntities())
            {
                var transform = entity.Get<Transform>();
                transform.Position = _input.MousePosition;
                entity.NotifyChanged<Transform>();
            }
        }
    }

    public class WorldService
    {
        public World? CurrentWorld { get; set; } = new World();

        public WorldService()
        {

        }
    }

    public class PreDrawSystem : BaseSystem<Batcher>
    {
        private readonly GraphicsDevice _graphicsDevice;

        public PreDrawSystem(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        public override void Update(Batcher state)
        {
            _graphicsDevice.Clear(Color.CornflowerBlue);
            state.Begin();
        }
    }

    public class PostDrawSystem : BaseSystem<Batcher>
    {

        public PostDrawSystem()
        {
        }

        public override void Update(Batcher state)
        {
            state.End();
        }
    }
}