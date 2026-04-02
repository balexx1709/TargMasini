using TargMasini.Entitati.Enums;

namespace TargMasini.Entitati;

/// <summary>
/// A doua entitate din aplicație: reprezintă un automobil din catalogul târgului.
/// Folosește aceleași enum-uri ca și TranzactieAuto.
/// </summary>
public class Auto
{
    public int         Id           { get; set; }
    public string      Firma        { get; set; } = string.Empty;
    public string      Model        { get; set; } = string.Empty;
    public int         AnFabricatie { get; set; }
    public CuloareAuto Culoare      { get; set; } = CuloareAuto.Negru;
    public OptiuniAuto Optiuni      { get; set; } = OptiuniAuto.Niciuna;
    public decimal     Pret         { get; set; }
    public bool        EsteVanduta  { get; set; } = false;
    public string      Descriere    { get; set; } = string.Empty;

    public Auto() { }

    public Auto(int id, string firma, string model, int anFabricatie,
                CuloareAuto culoare, OptiuniAuto optiuni, decimal pret,
                string descriere = "")
    {
        Id           = id;
        Firma        = firma;
        Model        = model;
        AnFabricatie = anFabricatie;
        Culoare      = culoare;
        Optiuni      = optiuni;
        Pret         = pret;
        Descriere    = descriere;
    }

    public string OptiuniText()
    {
        if (Optiuni == OptiuniAuto.Niciuna) return "Standard";
        return string.Join(", ",
            Enum.GetValues<OptiuniAuto>()
                .Where(o => o != OptiuniAuto.Niciuna && Optiuni.HasFlag(o)));
    }

    public override string ToString() =>
        $"[{Id:D3}] {Firma} {Model} ({AnFabricatie}) | {Culoare} | {Pret:C2}{(EsteVanduta ? " [VÂNDUT]" : " [DISPONIBIL]")}";
}
