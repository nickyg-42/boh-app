namespace ui.Models;

public class Conjugation
{
    public int Id { get; set; }
    public int VerbId { get; set; }
    public Verb? ParentVerb { get; set; }
    public string Group { get; set; } = string.Empty;
    public int GroupSort { get; set; }
    public int Sort { get; set; }
    public string Value { get; set; } = string.Empty;
    public string? Form { get; set; }
}
