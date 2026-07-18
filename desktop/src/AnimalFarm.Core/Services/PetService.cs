using AnimalFarm.Core.Models;
using AnimalFarm.Core.Storage;

namespace AnimalFarm.Core.Services;

public sealed class PetService
{
    private readonly PetRepository _repository;
    private readonly ImageService _images;

    public PetService(PetRepository repository, ImageService images)
    {
        _repository = repository;
        _images = images;
    }

    public IReadOnlyList<Pet> ListPets() => _repository.GetAll();

    public Pet? GetPet(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        return _repository.GetById(id);
    }

    public Pet CreatePet(string name, string? description, string photoPath)
    {
        if (string.IsNullOrWhiteSpace(photoPath) || !File.Exists(photoPath))
        {
            throw new InvalidOperationException("A photo is required.");
        }

        var trimmedName = name.Trim();
        if (trimmedName.Length == 0)
        {
            throw new InvalidOperationException("Name is required.");
        }

        var id = Guid.NewGuid().ToString("D");
        var imagePath = _images.SaveImportedImage(photoPath, id);
        var pet = new Pet
        {
            Id = id,
            Name = trimmedName,
            Description = (description ?? string.Empty).Trim(),
            ImagePath = imagePath,
            CreatedAt = DateTimeOffset.UtcNow,
        };

        _repository.Upsert(pet);
        return pet;
    }

    public Pet? UpdatePet(string id, string name, string? description, string? newPhotoPath)
    {
        var existing = _repository.GetById(id);
        if (existing is null)
        {
            return null;
        }

        var trimmedName = name.Trim();
        if (trimmedName.Length == 0)
        {
            throw new InvalidOperationException("Name is required.");
        }

        var imagePath = existing.ImagePath;
        if (!string.IsNullOrWhiteSpace(newPhotoPath) && File.Exists(newPhotoPath))
        {
            var previous = existing.ImagePath;
            imagePath = _images.SaveImportedImage(newPhotoPath, id);
            if (!string.Equals(previous, imagePath, StringComparison.OrdinalIgnoreCase))
            {
                _images.DeleteImage(previous);
            }
        }

        existing.Name = trimmedName;
        existing.Description = (description ?? string.Empty).Trim();
        existing.ImagePath = imagePath;
        _repository.Upsert(existing);
        return existing;
    }

    public bool DeletePet(string id)
    {
        var existing = _repository.GetById(id);
        if (existing is null)
        {
            return false;
        }

        var deleted = _repository.Delete(id);
        if (deleted)
        {
            _images.DeleteImage(existing.ImagePath);
        }

        return deleted;
    }
}
