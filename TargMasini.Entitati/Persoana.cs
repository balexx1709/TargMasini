namespace TargMasini.Entitati;

/// <summary>
/// Reprezintă o persoană (vânzător sau cumpărător) din târg.
/// </summary>
public class Persoana
{
    public int    Id      { get; set; }
    public string Nume    { get; set; } = string.Empty;
    public string Email   { get; set; } = string.Empty;
    public string Telefon { get; set; } = string.Empty;

    public Persoana() { }

    public Persoana(int id, string nume, string email, string telefon)
    {
        Id      = id;
        Nume    = nume;
        Email   = email;
        Telefon = telefon;
    }

    public override string ToString() =>
        $"[{Id:D3}] {Nume} | {Email} | {Telefon}";
}
