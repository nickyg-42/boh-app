namespace boh_api.Models;

public class RelatedWord
{
    public int Id { get; set; }
    public int VerbId { get; set; }
    public Verb? ParentVerb { get; set; }
    public string Word { get; set; } = string.Empty;
}