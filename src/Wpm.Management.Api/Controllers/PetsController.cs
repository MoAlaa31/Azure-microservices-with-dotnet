using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wpm.Management.Api.DataAccess;

namespace Wpm.Management.Api.Controllers;


public class PetsController(ManagementDbContext dbContext) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var allPets = await dbContext.Pets.ToListAsync();

        return Ok(allPets);
    }
}
