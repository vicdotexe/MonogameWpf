using Game.Shared.Scenes;
using Game.Shared.Systems;

using Microsoft.Extensions.DependencyInjection;

using Vez;
using Vez.Ecs;
using Vez.Graphics;

namespace Game.Shared
{
    public class Simple3DSetup : GameConfiguration
    {
        protected override void Configure(CoreConfiguration configuration, IServiceCollection services)
        {
            configuration.AddTime();
            configuration.Add<Simple3dScene>();
        }

        public Simple3DSetup(Action<CoreConfiguration, ServiceCollection> configuration) : base(configuration)
        {
        }
    }

    public class SpritePlayground : GameConfiguration
    {
        protected override void Configure(CoreConfiguration configuration, IServiceCollection services)
        {
            configuration.AddTime();
            configuration.Add<Batcher>();
            configuration.Add<TestScene>();
            configuration.Add<WorldService>();
            services.AddDefaultEcs();
        }

        public SpritePlayground(Action<CoreConfiguration, ServiceCollection> configuration) : base(configuration)
        {
        }
    }
}