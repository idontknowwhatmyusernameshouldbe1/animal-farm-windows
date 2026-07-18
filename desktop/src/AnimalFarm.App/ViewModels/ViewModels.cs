using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using AnimalFarm.App.Helpers;
using AnimalFarm.App.Services;
using AnimalFarm.Core.I18n;
using AnimalFarm.Core.Models;
using AnimalFarm.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AnimalFarm.App.ViewModels;

public sealed partial class MainViewModel : ObservableObject
{
    private readonly SettingsService _settings;
    private readonly NavigationService _navigation;
    private readonly PageFactory _pages;

    public MainViewModel(
        LocalizationService localization,
        SettingsService settings,
        NavigationService navigation,
        PageFactory pages)
    {
        Localization = localization;
        _settings = settings;
        _navigation = navigation;
        _pages = pages;

        Localization.Language = _settings.LoadLanguage();
        Localization.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName is nameof(LocalizationService.Language) or nameof(LocalizationService.T))
            {
                _settings.SaveLanguage(Localization.Language);
                OnPropertyChanged(nameof(Title));
            }
        };
    }

    public LocalizationService Localization { get; }

    public string Title => Localization.T.Brand;

    [RelayCommand]
    private void ToggleLanguage() => Localization.Toggle();

    [RelayCommand]
    private void GoHome() => _navigation.ClearHistoryAndNavigate(_pages.CreateHome());

    public void ShowHome() => GoHome();
}

public sealed partial class HomeViewModel : ObservableObject
{
    private readonly PetService _pets;
    private readonly NavigationService _navigation;
    private readonly PageFactory _pages;

    public HomeViewModel(
        PetService pets,
        LocalizationService localization,
        NavigationService navigation,
        PageFactory pages)
    {
        _pets = pets;
        Localization = localization;
        _navigation = navigation;
        _pages = pages;
    }

    public LocalizationService Localization { get; }

    public ObservableCollection<PetCardViewModel> Pets { get; } = [];

    public bool HasPets => Pets.Count > 0;

    public bool IsEmpty => Pets.Count == 0;

    [RelayCommand]
    private void AddPet() => _navigation.Navigate(_pages.CreateForm());

    [RelayCommand]
    private void OpenPet(PetCardViewModel? card)
    {
        if (card is null)
        {
            return;
        }

        _navigation.Navigate(_pages.CreateDetail(card.Id));
    }

    public void Refresh()
    {
        Pets.Clear();
        foreach (var pet in _pets.ListPets())
        {
            Pets.Add(new PetCardViewModel(pet));
        }

        OnPropertyChanged(nameof(HasPets));
        OnPropertyChanged(nameof(IsEmpty));
    }
}

public sealed class PetCardViewModel
{
    public PetCardViewModel(Pet pet)
    {
        Id = pet.Id;
        Name = pet.Name;
        Description = pet.Description;
        Image = ImageHelper.LoadBitmap(pet.ImagePath);
    }

    public string Id { get; }
    public string Name { get; }
    public string Description { get; }
    public BitmapImage? Image { get; }
}

public sealed partial class PetDetailViewModel : ObservableObject
{
    private readonly PetService _pets;
    private readonly NavigationService _navigation;
    private readonly PageFactory _pages;
    private string? _petId;

    public PetDetailViewModel(
        PetService pets,
        LocalizationService localization,
        NavigationService navigation,
        PageFactory pages)
    {
        _pets = pets;
        Localization = localization;
        _navigation = navigation;
        _pages = pages;
        Localization.PropertyChanged += (_, _) => RefreshComputed();
    }

    public LocalizationService Localization { get; }

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private BitmapImage? _image;

    [ObservableProperty]
    private bool _notFound;

    [ObservableProperty]
    private bool _isDeleting;

    public string DisplayDescription =>
        string.IsNullOrWhiteSpace(Description) ? Localization.T.NoDescription : Description;

    public string DeleteLabel => IsDeleting ? Localization.T.Removing : Localization.T.Delete;

    public void Load(string petId)
    {
        _petId = petId;
        var pet = _pets.GetPet(petId);
        if (pet is null)
        {
            NotFound = true;
            Name = string.Empty;
            Description = string.Empty;
            Image = null;
            RefreshComputed();
            return;
        }

        NotFound = false;
        Name = pet.Name;
        Description = pet.Description;
        Image = ImageHelper.LoadBitmap(pet.ImagePath);
        RefreshComputed();
    }

    [RelayCommand]
    private void Back() => _navigation.ClearHistoryAndNavigate(_pages.CreateHome());

    [RelayCommand]
    private void Edit()
    {
        if (string.IsNullOrWhiteSpace(_petId) || NotFound)
        {
            return;
        }

        _navigation.Navigate(_pages.CreateEditForm(_petId));
    }

    [RelayCommand]
    private void Delete()
    {
        if (string.IsNullOrWhiteSpace(_petId) || NotFound || IsDeleting)
        {
            return;
        }

        var confirm = System.Windows.MessageBox.Show(
            Localization.T.DeleteConfirm(Name),
            Localization.T.Brand,
            System.Windows.MessageBoxButton.YesNo,
            System.Windows.MessageBoxImage.Warning);

        if (confirm != System.Windows.MessageBoxResult.Yes)
        {
            return;
        }

        IsDeleting = true;
        OnPropertyChanged(nameof(DeleteLabel));
        try
        {
            if (!_pets.DeletePet(_petId))
            {
                System.Windows.MessageBox.Show(
                    Localization.T.DeleteFailed,
                    Localization.T.Brand,
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
                return;
            }

            _navigation.ClearHistoryAndNavigate(_pages.CreateHome());
        }
        finally
        {
            IsDeleting = false;
            OnPropertyChanged(nameof(DeleteLabel));
        }
    }

    private void RefreshComputed()
    {
        OnPropertyChanged(nameof(DisplayDescription));
        OnPropertyChanged(nameof(DeleteLabel));
    }
}

public sealed partial class PetFormViewModel : ObservableObject
{
    private readonly PetService _pets;
    private readonly NavigationService _navigation;
    private readonly PageFactory _pages;
    private string? _petId;
    private bool _isEdit;

    public PetFormViewModel(
        PetService pets,
        LocalizationService localization,
        NavigationService navigation,
        PageFactory pages)
    {
        _pets = pets;
        Localization = localization;
        _navigation = navigation;
        _pages = pages;
        Localization.PropertyChanged += (_, _) => RefreshTitles();
    }

    public LocalizationService Localization { get; }

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _subtitle = string.Empty;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private string? _selectedPhotoPath;

    [ObservableProperty]
    private BitmapImage? _previewImage;

    [ObservableProperty]
    private string _error = string.Empty;

    [ObservableProperty]
    private bool _isSaving;

    [ObservableProperty]
    private string _saveLabel = string.Empty;

    public bool HasPreview => PreviewImage is not null;

    public void LoadCreate()
    {
        _isEdit = false;
        _petId = null;
        Name = string.Empty;
        Description = string.Empty;
        SelectedPhotoPath = null;
        PreviewImage = null;
        Error = string.Empty;
        RefreshTitles();
        OnPropertyChanged(nameof(HasPreview));
    }

    public void LoadEdit(string petId)
    {
        _isEdit = true;
        _petId = petId;
        Error = string.Empty;

        var pet = _pets.GetPet(petId);
        if (pet is null)
        {
            Error = Localization.T.ErrorPetNotFound;
            RefreshTitles();
            return;
        }

        Name = pet.Name;
        Description = pet.Description;
        SelectedPhotoPath = null;
        PreviewImage = ImageHelper.LoadBitmap(pet.ImagePath);
        RefreshTitles();
        OnPropertyChanged(nameof(HasPreview));
    }

    [RelayCommand]
    private void ChoosePhoto()
    {
        var dialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.webp;*.tif;*.tiff|All files|*.*",
            Title = Localization.T.Photo,
        };

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        SelectedPhotoPath = dialog.FileName;
        PreviewImage = ImageHelper.LoadBitmap(dialog.FileName);
        Error = string.Empty;
        OnPropertyChanged(nameof(HasPreview));
    }

    [RelayCommand]
    private void Cancel()
    {
        if (_navigation.CanGoBack)
        {
            _navigation.GoBack();
            return;
        }

        _navigation.ClearHistoryAndNavigate(_pages.CreateHome());
    }

    [RelayCommand]
    private void Save()
    {
        if (IsSaving)
        {
            return;
        }

        Error = string.Empty;

        if (string.IsNullOrWhiteSpace(Name))
        {
            Error = Localization.T.ErrorNameRequired;
            return;
        }

        if (!_isEdit && string.IsNullOrWhiteSpace(SelectedPhotoPath))
        {
            Error = Localization.T.ErrorChoosePhoto;
            return;
        }

        IsSaving = true;
        SaveLabel = Localization.T.Saving;

        try
        {
            Pet? pet;
            if (_isEdit)
            {
                if (string.IsNullOrWhiteSpace(_petId))
                {
                    Error = Localization.T.ErrorPetNotFound;
                    return;
                }

                pet = _pets.UpdatePet(_petId, Name, Description, SelectedPhotoPath);
                if (pet is null)
                {
                    Error = Localization.T.ErrorPetNotFound;
                    return;
                }
            }
            else
            {
                pet = _pets.CreatePet(Name, Description, SelectedPhotoPath!);
            }

            _navigation.ClearHistoryAndNavigate(_pages.CreateDetail(pet.Id));
        }
        catch
        {
            Error = Localization.T.ErrorCouldNotSave;
        }
        finally
        {
            IsSaving = false;
            RefreshTitles();
        }
    }

    private void RefreshTitles()
    {
        if (_isEdit)
        {
            Title = string.IsNullOrWhiteSpace(Name)
                ? Localization.T.Edit
                : Localization.T.EditTitle(Name);
            Subtitle = Localization.T.EditSub;
            SaveLabel = IsSaving ? Localization.T.Saving : Localization.T.SaveChanges;
        }
        else
        {
            Title = Localization.T.AddPetTitle;
            Subtitle = Localization.T.AddPetSub;
            SaveLabel = IsSaving ? Localization.T.Saving : Localization.T.SavePet;
        }
    }
}
