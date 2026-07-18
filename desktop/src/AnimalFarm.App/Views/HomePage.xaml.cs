using AnimalFarm.App.ViewModels;

namespace AnimalFarm.App.Views;

public partial class HomePage
{
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        DataContext = viewModel;
    }

    public HomeViewModel ViewModel { get; }
}
