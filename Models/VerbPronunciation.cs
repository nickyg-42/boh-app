namespace boh_api.Models;

public class VerbPronunciation
{
    public int Id { get; set; }
    public int VerbId { get; set; }
    public Verb? ParentVerb { get; set; }
    public string Pronunciation { get; set; } = string.Empty;
}