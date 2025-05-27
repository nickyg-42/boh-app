using boh_api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class VerbsController : ControllerBase
{
    private readonly AppDbContext _context;

    public VerbsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("random")]
    public async Task<ActionResult<Verb>> GetRandomVerb()
    {
        var verb = await _context.Verbs
            .Include(v => v.Conjugations)
            .OrderBy(v => Guid.NewGuid())
            .FirstOrDefaultAsync();

        return verb != null ? verb : NotFound();
    }
}
