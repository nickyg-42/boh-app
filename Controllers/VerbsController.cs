using boh_api.Data;
using boh_api.Models;
using boh_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

[Route("api/[controller]")]
[ApiController]
public class VerbsController : ControllerBase
{
    private readonly AppDbContext _context;

    public VerbsController(AppDbContext context)
    {
        _context = context;
    }    [HttpGet("random")]
    public async Task<ActionResult<Verb>> GetRandomVerb()
    {
        var verb = await _context.Verbs
            .Include(v => v.Conjugations)
            .Include(v => v.Definitions)
            .Include(v => v.Pronunciations)
            .Include(v => v.RelatedWords)
            .OrderBy(r => EF.Functions.Random())
            .FirstOrDefaultAsync();

        if (verb == null) return NotFound();

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };

        return Ok(JsonSerializer.Serialize(verb, options));
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportVerbs([FromBody] string directoryPath)
    {
        try
        {
            var importService = HttpContext.RequestServices.GetRequiredService<VerbImportService>();
            await importService.ImportVerbsFromDirectory(directoryPath);
            return Ok("Import completed successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}