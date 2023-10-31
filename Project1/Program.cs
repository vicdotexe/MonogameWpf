using Game.Shared.Scenes;
using Game.Shared.Systems;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Vez;
using Vez.CoreServices;
using Vez.Ecs;
using Vez.Graphics;

using IHost host = CreateHostBuilder(args).Build();
host.Start();

var scope = host.Services.CreateScope();

var factory = scope.ServiceProvider.GetRequiredService<GameFactory>();

using var game = factory.CreateGame<VezGame>((core, services) =>
{
    core.AddTime();
    core.AddInput();
    
    core.Add<Batcher>();
    core.Add<TestScene>();
    core.Add<WorldService>();
    services.AddDefaultEcs();
});

game.Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<GameFactory>();
        });
