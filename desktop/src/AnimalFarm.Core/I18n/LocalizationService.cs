using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AnimalFarm.Core.I18n;

public sealed class LocalizationService : INotifyPropertyChanged
{
    private AppLanguage _language = AppLanguage.English;

    public event PropertyChangedEventHandler? PropertyChanged;

    public AppLanguage Language
    {
        get => _language;
        set
        {
            if (_language == value)
            {
                return;
            }

            _language = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(T));
            OnPropertyChanged(nameof(IsEnglish));
            OnPropertyChanged(nameof(IsSwedish));
        }
    }

    public bool IsEnglish => Language == AppLanguage.English;
    public bool IsSwedish => Language == AppLanguage.Swedish;

    public Strings T => Language == AppLanguage.Swedish ? Swedish : English;

    public void Toggle()
    {
        Language = Language == AppLanguage.English
            ? AppLanguage.Swedish
            : AppLanguage.English;
    }

    public void SetFromCode(string? code)
    {
        Language = string.Equals(code, "sv", StringComparison.OrdinalIgnoreCase)
            ? AppLanguage.Swedish
            : AppLanguage.English;
    }

    public string ToCode() => Language == AppLanguage.Swedish ? "sv" : "en";

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private static readonly Strings English = new()
    {
        Brand = "Animal Farm",
        LangToggle = "Svenska",
        LangToggleAria = "Switch language to Swedish",
        ThemeToDark = "Dark",
        ThemeToLight = "Light",
        ThemeToDarkAria = "Switch to dark mode",
        ThemeToLightAria = "Switch to light mode",
        HeroCopy = "Photos, names, and the small stories that make them yours.",
        AddPet = "Add a pet",
        LoadingFarm = "Loading your farm…",
        Loading = "Loading…",
        EmptyFarm = "No animals on the farm yet. Add your first pet to get started.",
        GalleryLabel = "Pet gallery",
        BackToFarm = "← Back to farm",
        AddPetTitle = "Add a pet",
        AddPetSub = "Upload a photo, give them a name, and write a short description.",
        PetNotFound = "Pet not found",
        PetNotFoundSub = "This pet is not in this app's farm data.",
        NoDescription = "No description yet.",
        Edit = "Edit",
        EditSub = "Update their photo, name, or description.",
        Photo = "Photo",
        PhotoPreview = "Preview",
        PhotoHintTitle = "Click to add a photo",
        PhotoHintBody = "Choose a picture from your computer",
        Name = "Name",
        NamePlaceholder = "e.g. Charlie",
        Description = "Description",
        DescriptionPlaceholder = "A little about this friend…",
        Saving = "Saving…",
        SavePet = "Save pet",
        SaveChanges = "Save changes",
        Cancel = "Cancel",
        ErrorChoosePhoto = "Please choose a photo.",
        ErrorNameRequired = "Name is required.",
        ErrorPetNotFound = "Pet not found.",
        ErrorCouldNotSave = "Could not save. Try another photo or try again.",
        Delete = "Delete",
        Removing = "Removing…",
        DeleteFailed = "Could not delete this pet. Try again.",
        EditTitle = name => $"Edit {name}",
        BackToPet = name => $"← Back to {name}",
        DeleteConfirm = name => $"Remove {name} from Animal Farm? This cannot be undone.",
    };

    private static readonly Strings Swedish = new()
    {
        Brand = "Animal Farm",
        LangToggle = "English",
        LangToggleAria = "Byt språk till engelska",
        ThemeToDark = "Mörkt",
        ThemeToLight = "Ljust",
        ThemeToDarkAria = "Byt till mörkt läge",
        ThemeToLightAria = "Byt till ljust läge",
        HeroCopy = "Foton, namn och de små berättelserna som gör dem till era.",
        AddPet = "Lägg till husdjur",
        LoadingFarm = "Laddar din gård…",
        Loading = "Laddar…",
        EmptyFarm = "Inga djur på gården ännu. Lägg till ditt första husdjur för att komma igång.",
        GalleryLabel = "Husdjursgalleri",
        BackToFarm = "← Tillbaka till gården",
        AddPetTitle = "Lägg till husdjur",
        AddPetSub = "Ladda upp ett foto, ge dem ett namn och skriv en kort beskrivning.",
        PetNotFound = "Husdjur hittades inte",
        PetNotFoundSub = "Det här husdjuret finns inte i den här appens data.",
        NoDescription = "Ingen beskrivning ännu.",
        Edit = "Redigera",
        EditSub = "Uppdatera foto, namn eller beskrivning.",
        Photo = "Foto",
        PhotoPreview = "Förhandsvisning",
        PhotoHintTitle = "Klicka för att lägga till foto",
        PhotoHintBody = "Välj en bild från din dator",
        Name = "Namn",
        NamePlaceholder = "t.ex. Charlie",
        Description = "Beskrivning",
        DescriptionPlaceholder = "Lite om den här vännen…",
        Saving = "Sparar…",
        SavePet = "Spara husdjur",
        SaveChanges = "Spara ändringar",
        Cancel = "Avbryt",
        ErrorChoosePhoto = "Välj ett foto.",
        ErrorNameRequired = "Namn krävs.",
        ErrorPetNotFound = "Husdjur hittades inte.",
        ErrorCouldNotSave = "Kunde inte spara. Prova ett annat foto eller försök igen.",
        Delete = "Ta bort",
        Removing = "Tar bort…",
        DeleteFailed = "Kunde inte ta bort husdjuret. Försök igen.",
        EditTitle = name => $"Redigera {name}",
        BackToPet = name => $"← Tillbaka till {name}",
        DeleteConfirm = name => $"Ta bort {name} från Animal Farm? Det går inte att ångra.",
    };
}
