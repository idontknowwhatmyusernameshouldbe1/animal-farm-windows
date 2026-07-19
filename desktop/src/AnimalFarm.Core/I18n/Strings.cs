namespace AnimalFarm.Core.I18n;

public sealed class Strings
{
    public required string Brand { get; init; }
    public required string LangToggle { get; init; }
    public required string LangToggleAria { get; init; }
    public required string ThemeToDark { get; init; }
    public required string ThemeToLight { get; init; }
    public required string ThemeToDarkAria { get; init; }
    public required string ThemeToLightAria { get; init; }
    public required string HeroCopy { get; init; }
    public required string AddPet { get; init; }
    public required string LoadingFarm { get; init; }
    public required string Loading { get; init; }
    public required string EmptyFarm { get; init; }
    public required string GalleryLabel { get; init; }
    public required string BackToFarm { get; init; }
    public required string AddPetTitle { get; init; }
    public required string AddPetSub { get; init; }
    public required string PetNotFound { get; init; }
    public required string PetNotFoundSub { get; init; }
    public required string NoDescription { get; init; }
    public required string Edit { get; init; }
    public required string EditSub { get; init; }
    public required string Photo { get; init; }
    public required string PhotoPreview { get; init; }
    public required string PhotoHintTitle { get; init; }
    public required string PhotoHintBody { get; init; }
    public required string Name { get; init; }
    public required string NamePlaceholder { get; init; }
    public required string Description { get; init; }
    public required string DescriptionPlaceholder { get; init; }
    public required string Saving { get; init; }
    public required string SavePet { get; init; }
    public required string SaveChanges { get; init; }
    public required string Cancel { get; init; }
    public required string ErrorChoosePhoto { get; init; }
    public required string ErrorNameRequired { get; init; }
    public required string ErrorPetNotFound { get; init; }
    public required string ErrorCouldNotSave { get; init; }
    public required string Delete { get; init; }
    public required string Removing { get; init; }
    public required string DeleteFailed { get; init; }

    public required Func<string, string> EditTitle { get; init; }
    public required Func<string, string> BackToPet { get; init; }
    public required Func<string, string> DeleteConfirm { get; init; }
}
