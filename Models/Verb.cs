public class Verb
{
    public int Id { get; set; }
    public string Infinitive { get; set; } = string.Empty;
    public bool IsWeakVerb { get; set; }

    public ICollection<Conjugation> Conjugations { get; set; } = new List<Conjugation>();
}
