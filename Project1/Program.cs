﻿using Game.Shared;
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

using var game = factory.CreateGame<VezGame, SpritePlayground>((core, services) =>
{
    core.AddInput();
});

game.Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<GameFactory>();
        });
