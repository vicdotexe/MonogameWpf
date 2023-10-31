using CommunityToolkit.Mvvm.Messaging;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using System.Windows.Threading;

using Vez;
using Game.Shared.Scenes;
using Game.Shared.Systems;

using Vez.Ecs;
using Vez.Graphics;

namespace MgEditor;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    [STAThread]
    public static void Main(string[] args)
    {
        using IHost host = CreateHostBuilder(args).Build();
        host.Start();

        App app = new();
        app.InitializeComponent();
        app.MainWindow = host.Services.GetRequiredService<MainWindow>();
        app.MainWindow.Visibility = Visibility.Visible;
        app.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder)
            => configurationBuilder.AddUserSecrets(typeof(App).Assembly))
        .ConfigureServices((hostContext, services) =>
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();

            services.AddSingleton<WeakReferenceMessenger>();
            services.AddSingleton<IMessenger, WeakReferenceMessenger>(provider => provider.GetRequiredService<WeakReferenceMessenger>());

            services.AddSingleton(_ => Current.Dispatcher);

            services.AddTransient<ISnackbarMessageQueue>(provider =>
            {
                Dispatcher dispatcher = provider.GetRequiredService<Dispatcher>();
                return new SnackbarMessageQueue(TimeSpan.FromSeconds(3.0), dispatcher);
            });

            services.AddSingleton<GameFactory>();
            AddGameViewModel(services);
            Add3dGameViewModel(services);
        });


    static void AddGameViewModel(IServiceCollection services)
    {
        services.AddVez<GameViewModel>(x =>
        {
            x.AddTime();
            x.AddInput<WpfInputService>();

            x.Add<Batcher>();
            x.Add<TestScene>();
            x.Add<WorldService>();
        });

        services.AddDefaultEcs();
    }

    static void Add3dGameViewModel(IServiceCollection services)
    {
        services.AddVez<Simple3dViewModel>(x =>
        {
            x.AddTime();
            x.AddInput<WpfInputService>();

            x.Add<Simple3dScene>();
        });

        services.AddDefaultEcs();
    }   

}

public class Simple3dViewModel : GameViewModel
{
    public Simple3dViewModel(IServiceProvider services) : base(services)
    {
    }
}
