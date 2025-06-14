namespace ui.Models;

public class Verb
{    
    public int Id { get; set; }
    public string Infinitive { get; set; } = string.Empty;
    public string? Etymology { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public int? PageId { get; set; }
    public string? Url { get; set; }
    public List<VerbDefinition> Definitions { get; set; } = new();
    public List<Tag> Tags { get; set; } = new();
    public List<Conjugation> Conjugations { get; set; } = new();
    public List<VerbPronunciation> Pronunciations { get; set; } = new();
    public List<RelatedWord> RelatedWords { get; set; } = new();
}