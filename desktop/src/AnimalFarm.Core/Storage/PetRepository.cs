using AnimalFarm.Core.Models;
using Microsoft.Data.Sqlite;

namespace AnimalFarm.Core.Storage;

public sealed class PetRepository
{
    private readonly AppPaths _paths;
    private readonly string _connectionString;

    public PetRepository(AppPaths paths)
    {
        _paths = paths;
        _connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = paths.DatabasePath,
        }.ToString();
    }

    public void Initialize()
    {
        _paths.EnsureCreated();

        using var connection = Open();
        using var command = connection.CreateCommand();
        command.CommandText =
            """
            CREATE TABLE IF NOT EXISTS pets (
                Id TEXT NOT NULL PRIMARY KEY,
                Name TEXT NOT NULL,
                Description TEXT NOT NULL,
                ImagePath TEXT NOT NULL,
                CreatedAt TEXT NOT NULL
            );
            """;
        command.ExecuteNonQuery();
    }

    public IReadOnlyList<Pet> GetAll()
    {
        using var connection = Open();
        using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, Name, Description, ImagePath, CreatedAt
            FROM pets
            ORDER BY CreatedAt DESC;
            """;

        var pets = new List<Pet>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            pets.Add(ReadPet(reader));
        }

        return pets;
    }

    public Pet? GetById(string id)
    {
        using var connection = Open();
        using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, Name, Description, ImagePath, CreatedAt
            FROM pets
            WHERE Id = $id
            LIMIT 1;
            """;
        command.Parameters.AddWithValue("$id", id);

        using var reader = command.ExecuteReader();
        return reader.Read() ? ReadPet(reader) : null;
    }

    public void Upsert(Pet pet)
    {
        using var connection = Open();
        using var command = connection.CreateCommand();
        command.CommandText =
            """
            INSERT INTO pets (Id, Name, Description, ImagePath, CreatedAt)
            VALUES ($id, $name, $description, $imagePath, $createdAt)
            ON CONFLICT(Id) DO UPDATE SET
                Name = excluded.Name,
                Description = excluded.Description,
                ImagePath = excluded.ImagePath;
            """;
        command.Parameters.AddWithValue("$id", pet.Id);
        command.Parameters.AddWithValue("$name", pet.Name);
        command.Parameters.AddWithValue("$description", pet.Description);
        command.Parameters.AddWithValue("$imagePath", pet.ImagePath);
        command.Parameters.AddWithValue("$createdAt", pet.CreatedAt.ToString("O"));
        command.ExecuteNonQuery();
    }

    public bool Delete(string id)
    {
        using var connection = Open();
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM pets WHERE Id = $id;";
        command.Parameters.AddWithValue("$id", id);
        return command.ExecuteNonQuery() > 0;
    }

    private SqliteConnection Open()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }

    private static Pet ReadPet(SqliteDataReader reader) =>
        new()
        {
            Id = reader.GetString(0),
            Name = reader.GetString(1),
            Description = reader.GetString(2),
            ImagePath = reader.GetString(3),
            CreatedAt = DateTimeOffset.Parse(reader.GetString(4)),
        };
}
