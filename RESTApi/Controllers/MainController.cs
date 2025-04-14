using Microsoft.AspNetCore.Mvc;
using RESTApi.Contracts.Requests;
using RESTApi.Data;
using RESTApi.Models;

namespace RESTApi.Controllers;

[ApiController]
[Route("api/animals")]
public class MainController : ControllerBase
{
    private readonly List<Animal> _animals = AnimalsRepository.Animals;
    private readonly List<Visit> _visits = VisitsRepository.Visits;
    
    // Animals ---------------------------------------------------------------------------------------------------------
    
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_animals);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var animal = _animals.FirstOrDefault(a => a.Id == id);
        if (animal is null) return NotFound();
        return Ok(animal);
    }
    
    [HttpPost]
    public IActionResult Create([FromBody] CreateAnimalRequest animalToCreate)
    {
        var id = _animals.Max(a => a.Id) + 1;
        var animal = new Animal
        {
            Id = id,
            Name = animalToCreate.Name,
            Category = animalToCreate.Category,
            Weight = animalToCreate.Weight,
            FurColor = animalToCreate.FurColor
        };
        _animals.Add(animal);
        return CreatedAtAction(nameof(Get), new { id }, animal);
    }
    
    [HttpPut("{id:int}")]
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateAnimalRequest animalToUpdate)
    {
        var animal = _animals.FirstOrDefault(a => a.Id == id);
        if (animal is null) return NotFound();
        animal.Name = animalToUpdate.Name;
        // Category can't be updated imo
        animal.Weight = animalToUpdate.Weight;
        animal.FurColor = animalToUpdate.FurColor;
        return Ok(animal);
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var animal = _animals.FirstOrDefault(a => a.Id == id);
        if (animal is null) return NotFound();
        _animals.Remove(animal);
        return NoContent();
    }
    
    // Visits ----------------------------------------------------------------------------------------------------------

    [HttpGet("{id:int}/visits")]
    public IActionResult GetVisits(int id)
    {
        var animal = _animals.FirstOrDefault(a => a.Id == id);
        if (animal is null) return NotFound();
        var visits = _visits.Where(v => v.AnimalId == id).ToList();
        return Ok(visits);
    }

    [HttpPost("{animalId:int}/visits")]
    public IActionResult CreateVisit([FromRoute] int animalId, [FromBody] CreateVisitRequest animalToCreate)
    {
        if (_animals.All(a => a.Id != animalId)) return BadRequest();

        var visit = new Visit
        {
            Id = _visits.Max(a => a.Id) + 1,
            DateOfVisit = animalToCreate.DateOfVisit,
            AnimalId = animalId,
            Description = animalToCreate.Description,
            Price = animalToCreate.Price
        };
        
        _visits.Add(visit);
        return CreatedAtAction(nameof(GetVisits), new { id = animalId }, visit);
    }
}