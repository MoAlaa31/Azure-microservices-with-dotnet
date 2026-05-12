using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wpm.Management.Api.DataAccess;

namespace Wpm.Management.Api.Controllers;


public class PetsController(ManagementDbContext dbContext) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<List<Pet>>> Get()
    {
        var allPets = await dbContext.Pets.Include(p => p.Breed).ToListAsync();

        return Ok(allPets);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Pet>> GetById(int id)
    {
        var pet = await dbContext.Pets.Include(p => p.Breed)
            .Where(p => p.Id == id).FirstOrDefaultAsync();
        if (pet is null)
        {
            return NotFound();
        }
        return Ok(pet);
    }

    [HttpPost]
    public async Task<ActionResult> Create(NewPet newPet)
    {
        var pet = newPet.ToPet();
        await dbContext.Pets.AddAsync(pet);
        await dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = pet.Id }, pet);
    }
}

public record NewPet(string Name, int Age, int BreedId)
{
    public Pet ToPet() => new Pet { Name = Name, Age = Age, BreedId = BreedId };
}