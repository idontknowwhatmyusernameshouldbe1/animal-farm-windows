using System.Windows;
using AnimalFarm.App.Services;
using AnimalFarm.App.ViewModels;

namespace AnimalFarm.App;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel, NavigationService navigation)
    {
        InitializeComponent();
        DataContext = viewModel;
        navigation.Attach(RootFrame);
    }
}
