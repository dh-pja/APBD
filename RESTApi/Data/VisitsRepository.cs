using RESTApi.Models;

namespace RESTApi.Data;

public static class VisitsRepository
{
    public static readonly List<Visit> Visits = new()
    {
        new Visit
        {
            Id = 1,
            DateOfVisit = DateTime.Now.Subtract(TimeSpan.FromDays(12)),
            AnimalId = 3,
            Description = "Diagnostic visit",
            Price = 100
        },
        new Visit
        {
            Id = 2,
            DateOfVisit = DateTime.Now.Subtract(TimeSpan.FromDays(10)),
            AnimalId = 2,
            Price = 200
        },
        new Visit
        {
            Id = 3,
            DateOfVisit = DateTime.Now.Subtract(TimeSpan.FromDays(4)),
            AnimalId = 1,
            Description = "Check-up visit",
            Price = 300
        },
        new Visit
        {
            Id = 4,
            DateOfVisit = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
            AnimalId = 3,
            Description = "Surgery",
            Price = 400
        },
    };
}