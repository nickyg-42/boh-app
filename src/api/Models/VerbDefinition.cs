namespace boh_api.Models;

public class VerbDefinition
{
    public int Id { get; set; }
    public int VerbId { get; set; }
    public Verb? ParentVerb { get; set; }
    public string Definition { get; set; } = string.Empty;
}