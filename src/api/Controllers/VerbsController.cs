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
    }

    [HttpGet("random")]
    public async Task<ActionResult<Verb>> GetRandomVerb()
    {
        var verb = await _context.Verbs
            .Include(v => v.Conjugations)
            .Include(v => v.Definitions)
            .Include(v => v.Pronunciations)
            .Include(v => v.RelatedWords)
            .Include(v => v.Tags)
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

    [HttpGet("search/byname")]
    public async Task<ActionResult<IEnumerable<Verb>>> SearchVerbsByVerbName([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Search query is required");
        }

        var verbs = await _context.Verbs
            .Where(v => v.Infinitive != null && v.Infinitive.StartsWith(name))
            .Include(v => v.Conjugations)
            .Include(v => v.Definitions)
            .Include(v => v.Pronunciations)
            .Include(v => v.RelatedWords)
            .Include(v => v.Tags)
            .Take(10)
            .ToListAsync();

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };

        return Ok(JsonSerializer.Serialize(verbs, options));
    }

    [HttpGet("search/bydef")]
    public async Task<ActionResult<IEnumerable<Verb>>> SearchVerbsByDefinition([FromQuery] string definition)
    {
        if (string.IsNullOrWhiteSpace(definition))
        {
            return BadRequest("Search query is required");
        }

        var verbs = await _context.Verbs
            .Where(v => v.Definitions != null && v.Definitions.Any(d => d.Definition.Contains(definition)))
            .Include(v => v.Conjugations)
            .Include(v => v.Definitions)
            .Include(v => v.Pronunciations)
            .Include(v => v.RelatedWords)
            .Include(v => v.Tags)
            .Take(10)
            .ToListAsync();

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };

        return Ok(JsonSerializer.Serialize(verbs, options));
    }

    [HttpGet("search/bytag")]
    public async Task<ActionResult<IEnumerable<Verb>>> SearchVerbsByTag([FromQuery] string tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
        {
            return BadRequest("Tag is required");
        }

        var lowercaseTag = tag.ToLower();
        var verbs = await _context.Verbs
            .Where(v => v.Tags != null && v.Tags.Any(t => t.Name.Contains(lowercaseTag)))
            .Include(v => v.Conjugations)
            .Include(v => v.Definitions)
            .Include(v => v.Pronunciations)
            .Include(v => v.RelatedWords)
            .Include(v => v.Tags)
            .Take(10)
            .ToListAsync();

        return Ok(verbs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Verb>> GetVerb(int id)
    {
        var verb = await _context.Verbs
            .Include(v => v.Conjugations)
            .Include(v => v.Definitions)
            .Include(v => v.Pronunciations)
            .Include(v => v.RelatedWords)
            .Include(v => v.Tags)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (verb == null)
        {
            return NotFound();
        }

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };

        return Ok(JsonSerializer.Serialize(verb, options));
    }

    [HttpPatch("{id}/tags")]
    public async Task<IActionResult> UpdateTags(int id, [FromBody] ICollection<string> tagNames)
    {
        var verb = await _context.Verbs
            .Include(v => v.Tags)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (verb == null)
        {
            return NotFound();
        }

        // Clear existing tags
        verb.Tags?.Clear();

        // Add new tags
        foreach (var tagName in tagNames)
        {
            var tag = await _context.Set<Tag>()
                .FirstOrDefaultAsync(t => t.Name == tagName);

            if (tag == null)
            {
                tag = new Tag { Name = tagName };
                _context.Add(tag);
            }

            verb.Tags?.Add(tag);
        }

        await _context.SaveChangesAsync();
        return Ok(verb);
    }

    [HttpPatch("{id}/tags/add")]
    public async Task<IActionResult> AddTag(int id, [FromQuery] string tagName)
    {
        var verb = await _context.Verbs
            .Include(v => v.Tags)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (verb == null)
        {
            return NotFound();
        }

        var tag = await _context.Set<Tag>()
                .FirstOrDefaultAsync(t => t.Name == tagName);

        if (tag == null)
        {
            tag = new Tag { Name = tagName };
            _context.Add(tag);
        }

        verb.Tags?.Add(tag);

        await _context.SaveChangesAsync();

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };

        return Ok(JsonSerializer.Serialize(verb, options));
    }

    [HttpPatch("{id}/tags/del")]
    public async Task<IActionResult> DeleteTag(int id, [FromQuery] string tagName)
    {
        var verb = await _context.Verbs
            .Include(v => v.Tags)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (verb == null)
        {
            return NotFound();
        }

        var tag = verb.Tags?.FirstOrDefault(t => t.Name == tagName);
        if (tag != null)
        {
            verb.Tags?.Remove(tag);
            await _context.SaveChangesAsync();
        }

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };

        return Ok(JsonSerializer.Serialize(verb, options));
    }

    [HttpPost("{id}/tags/clear")]
    public async Task<IActionResult> ClearTags(int id)
    {
        var verb = await _context.Verbs
            .Include(v => v.Tags)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (verb == null)
        {
            return NotFound();
        }

        verb.Tags = new List<Tag>();
        await _context.SaveChangesAsync();

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };

        return Ok(JsonSerializer.Serialize(verb, options));
    }
}