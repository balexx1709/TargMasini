namespace TargMasini.Entitati;

/// <summary>
/// Reprezintă un automobil disponibil în târg.
/// </summary>
public class Auto
{
    public int          Id           { get; set; }
    public string       Firma        { get; set; } = string.Empty;
    public string       Model        { get; set; } = string.Empty;
    public int          AnFabricatie { get; set; }
    public string       Culoare      { get; set; } = string.Empty;
    public List<string> Optiuni      { get; set; } = new();
    public decimal      Pret         { get; set; }
    public bool         EsteVanduta  { get; set; } = false;

    public Auto() { }

    public Auto(int id, string firma, string model, int anFabricatie,
                string culoare, List<string> optiuni, decimal pret)
    {
        Id           = id;
        Firma        = firma;
        Model        = model;
        AnFabricatie = anFabricatie;
        Culoare      = culoare;
        Optiuni      = optiuni;
        Pret         = pret;
    }

    public override string ToString() =>
        $"[{Id:D3}] {Firma} {Model} ({AnFabricatie}) - {Culoare} - {Pret:C2}{(EsteVanduta ? " [VÂNDUT]" : "")}";
}
