public class Conjugation
{
    public int Id { get; set; }
    public int VerbId { get; set; }
    public Verb Verb { get; set; } = null!;
    
    public string Tense { get; set; } = string.Empty;
    public string Mood { get; set; } = string.Empty;
    public string Person { get; set; } = string.Empty;
    public string ConjugatedForm { get; set; } = string.Empty;
}
