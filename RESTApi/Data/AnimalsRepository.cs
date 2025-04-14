using RESTApi.Models;

namespace RESTApi.Data;

public static class AnimalsRepository
{
    public static readonly List<Animal> Animals = new()
    {
        new Animal
        {
            Id = 1,
            Name = "Bobby",
            Category = "Dog",
            Weight = 20.5,
            FurColor = "Brown"
        },
        new Animal
        {
            Id = 2,
            Name = "Mif",
            Category = "Cat",
            Weight = 10.0,
            FurColor = "Black"
        },
        new Animal
        {
            Id = 3,
            Name = "Mimi",
            Category = "Cat",
            Weight = 8.0,
            FurColor = "Beige"
        },
        new Animal
        {
            Id = 4,
            Name = "Paul",
            Category = "Spider",
            Weight = 0.27,
            FurColor = "Golden"
        },  
    };
}