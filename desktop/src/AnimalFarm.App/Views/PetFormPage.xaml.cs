using AnimalFarm.App.ViewModels;

namespace AnimalFarm.App.Views;

public partial class PetFormPage
{
    public PetFormPage(PetFormViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        DataContext = viewModel;
    }

    public PetFormViewModel ViewModel { get; }
}
