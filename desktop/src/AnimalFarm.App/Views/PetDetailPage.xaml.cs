using AnimalFarm.App.ViewModels;

namespace AnimalFarm.App.Views;

public partial class PetDetailPage
{
    public PetDetailPage(PetDetailViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        DataContext = viewModel;
    }

    public PetDetailViewModel ViewModel { get; }
}
