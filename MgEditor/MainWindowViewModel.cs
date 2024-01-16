using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Game.Shared.Scenes;
using Game.Shared.Systems;

using Monogame.Wpf;

using Vez;
using Vez.CoreServices.Inputs;
using Vez.Ecs;
using Vez.Graphics;

namespace MgEditor;

public partial class MainWindowViewModel : ObservableObject
{
    //This is using the source generators from CommunityToolkit.Mvvm to generate a RelayCommand
    //See: https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/generators/observableproperty
    //and: https://learn.microsoft.com/windows/communitytoolkit/mvvm/observableobject
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(IncrementCountCommand))]
    private int _count;
    
    public MainWindowViewModel(GameFactory factory)
    {
        GameViewModel = factory.CreateGame<GameViewModel>((core, services) =>
        {
            core.AddTime();
            core.AddInput<WpfInputService>();

            core.Add<Batcher>();
            core.Add<TestScene>();
            core.Add<WorldService>();

            services.AddDefaultEcs();
        });

        GameViewModel2 = factory.CreateGame<GameViewModel>((core, services) =>
        {
            core.AddTime();
            core.AddInput<WpfInputService>();

            core.Add<Simple3dScene>();
        });
    }

    //This is using the source generators from CommunityToolkit.Mvvm to generate a RelayCommand
    //See: https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/generators/relaycommand
    //and: https://learn.microsoft.com/windows/communitytoolkit/mvvm/relaycommand
    [RelayCommand(CanExecute = nameof(CanIncrementCount))]
    private void IncrementCount()
    {
        Count++;
    }

    private bool CanIncrementCount() => Count < 5;

    [RelayCommand]
    private void ClearCount()
    {
        Count = 0;
    }

    [ObservableProperty]
    private IMonoGameViewModel? _gameViewModel;

    [ObservableProperty]
    private IMonoGameViewModel? _gameViewModel2;
}

