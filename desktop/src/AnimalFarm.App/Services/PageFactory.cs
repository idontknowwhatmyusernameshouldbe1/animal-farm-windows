using AnimalFarm.App.Services;
using AnimalFarm.App.ViewModels;
using AnimalFarm.App.Views;
using Microsoft.Extensions.DependencyInjection;

namespace AnimalFarm.App.Services;

public sealed class PageFactory
{
    private readonly IServiceProvider _services;

    public PageFactory(IServiceProvider services)
    {
        _services = services;
    }

    public HomePage CreateHome()
    {
        var page = _services.GetRequiredService<HomePage>();
        page.ViewModel.Refresh();
        return page;
    }

    public PetDetailPage CreateDetail(string petId)
    {
        var page = _services.GetRequiredService<PetDetailPage>();
        page.ViewModel.Load(petId);
        return page;
    }

    public PetFormPage CreateForm()
    {
        var page = _services.GetRequiredService<PetFormPage>();
        page.ViewModel.LoadCreate();
        return page;
    }

    public PetFormPage CreateEditForm(string petId)
    {
        var page = _services.GetRequiredService<PetFormPage>();
        page.ViewModel.LoadEdit(petId);
        return page;
    }
}
