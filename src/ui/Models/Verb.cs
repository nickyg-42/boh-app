namespace ui.Models;

public class Verb
{    
    public int Id { get; set; }
    public string Infinitive { get; set; } = string.Empty;
    public string? Etymology { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public List<VerbDefinition> Definitions { get; set; } = new();
    public List<Tag> Tags { get; set; } = new();
}