using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wpm.Management.Api.DataAccess;

namespace Wpm.Management.Api.Controllers;

public class BreedsController(ManagementDbContext dbContext) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<List<Breed>>> Get()
    {
        var breeds = await dbContext.Breeds
            .OrderBy(b => b.Id)
            .ToListAsync();

        return Ok(breeds);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Breed>> GetById(int id)
    {
        var breed = await dbContext.Breeds.FirstOrDefaultAsync(b => b.Id == id);
        if (breed is null)
        {
            return NotFound();
        }

        return Ok(breed);
    }

    [HttpPost]
    public async Task<ActionResult<Breed>> Create(NewBreed newBreed)
    {
        var maxId = await dbContext.Breeds.MaxAsync(b => (int?)b.Id) ?? 0;
        var breed = new Breed(maxId + 1, newBreed.Name);

        await dbContext.Breeds.AddAsync(breed);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = breed.Id }, breed);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var breed = await dbContext.Breeds.FirstOrDefaultAsync(b => b.Id == id);
        if (breed is null)
        {
            return NotFound();
        }

        dbContext.Breeds.Remove(breed);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}

public record NewBreed(string Name)
{
    public Breed ToBreed(int id) => new Breed(id, Name);
}
