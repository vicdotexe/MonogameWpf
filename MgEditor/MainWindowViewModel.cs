using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Monogame.Wpf;

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
        GameViewModel = factory.Create<GameViewModel>();
        GameViewModel2 = factory.Create<Simple3dViewModel>();
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
