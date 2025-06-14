
namespace boh_api.Services;

using System.Text.Json;
using System.Text.Json.Serialization;
using boh_api.Data;
using boh_api.Models;

public class VerbImportService
{
    private readonly AppDbContext _context;
    private readonly ILogger<VerbImportService> _logger;

    public VerbImportService(AppDbContext context, ILogger<VerbImportService> logger)
    {
        _context = context;
        _logger = logger;
    }

 public class VerbJson
{
    [JsonPropertyName("conjugations")]
    public List<ConjugationJson> Conjugations { get; set; } = new();
    
    [JsonPropertyName("definitions")]
    public List<string> Definitions { get; set; } = new();
    
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
    
    [JsonPropertyName("etymology")]
    public string Etymology { get; set; } = string.Empty;
    
    [JsonPropertyName("page_id")]
    public int PageId { get; set; }
    
    [JsonPropertyName("pronunciations")]
    public List<string> Pronunciations { get; set; } = new();
    
    [JsonPropertyName("related")]
    public List<string> Related { get; set; } = new();
    
    [JsonPropertyName("synonyms")]
    public List<string> Synonyms { get; set; } = new();
    
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
    
    [JsonPropertyName("word")]
    public string Word { get; set; } = string.Empty;
}

    public class ConjugationJson
    {
        public string? Form { get; set; }
        public string Group { get; set; } = string.Empty;
        [JsonPropertyName("group_sort")]
        public int GroupSort { get; set; }
        public int Sort { get; set; }
        public string Value { get; set; } = string.Empty;
    }

    public async Task ImportVerbsFromDirectory(string directoryPath)
    {
        _logger.LogInformation("Starting import of verbs from directory: {DirectoryPath}", directoryPath);
        foreach (var file in Directory.EnumerateFiles(directoryPath, "*.json", SearchOption.AllDirectories))
        {
            try
            {
                var jsonContent = await File.ReadAllTextAsync(file);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                var verbJson = JsonSerializer.Deserialize<VerbJson>(jsonContent, options);
                if (verbJson == null)
                {
                    _logger.LogError("Failed to deserialize JSON content: {JsonContent}", jsonContent);
                    continue;
                }

                var verb = new Verb
                {
                    Infinitive = verbJson.Word,
                    Etymology = verbJson.Etymology,
                    Description = verbJson.Description,
                    PageId = verbJson.PageId,
                    Url = verbJson.Url
                };

                // Add conjugations
                foreach (var conj in verbJson.Conjugations)
                {
                    verb.Conjugations.Add(new Conjugation
                    {
                        Form = conj.Form,
                        Group = conj.Group,
                        GroupSort = conj.GroupSort,
                        Sort = conj.Sort,
                        Value = conj.Value
                    });
                }

                // Add definitions
                foreach (var def in verbJson.Definitions)
                {
                    verb.Definitions.Add(new VerbDefinition
                    {
                        Definition = def
                    });
                }

                // Add pronunciations
                foreach (var pron in verbJson.Pronunciations)
                {
                    verb.Pronunciations.Add(new VerbPronunciation
                    {
                        Pronunciation = pron
                    });
                }

                // Add related words
                foreach (var related in verbJson.Related)
                {
                    verb.RelatedWords.Add(new RelatedWord
                    {
                        Word = related
                    });
                }

                _context.Verbs.Add(verb);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Imported verb: {verb.Infinitive}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error importing file: {Path.GetFileName(file)}");
            }
        }
    }
}
