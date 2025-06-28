namespace ui.Models;

public class Verb
{    
    public int Id { get; set; }
    public string Infinitive { get; set; } = string.Empty;
    public string? Etymology { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public int? PageId { get; set; }
    public string? Url { get; set; }
    public ICollection<VerbDefinition> Definitions { get; set; } = new List<VerbDefinition>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public ICollection<Conjugation> Conjugations { get; set; } = new List<Conjugation>();
    public ICollection<VerbPronunciation> Pronunciations { get; set; } = new List<VerbPronunciation>();
    public ICollection<RelatedWord> RelatedWords { get; set; } = new List<RelatedWord>();
}