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

using var game = scope.ServiceProvider.GetRequiredService<IGame>();
game.Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            AddGame(services);
        });




static void AddGame(IServiceCollection services)
{
    services.AddVez<VezGame>(x =>
    {
        x.AddTime();
        x.AddInput();

        x.Add<Batcher>();
        x.Add<TestScene>();
        x.Add<WorldService>();
    });

    services.AddDefaultEcs();
}