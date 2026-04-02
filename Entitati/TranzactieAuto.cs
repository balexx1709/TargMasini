namespace TargMasini.Entitati;

/// <summary>
/// Reprezintă o tranzacție de vânzare/cumpărare a unui automobil.
/// </summary>
public class TranzactieAuto
{
    // ─── Identificare ───────────────────────────────────────────────────────
    public int Id { get; set; }

    // ─── Participanți ───────────────────────────────────────────────────────
    public string NumeVanzator    { get; set; } = string.Empty;
    public string NumeCumparator  { get; set; } = string.Empty;

    // ─── Detalii vehicul ────────────────────────────────────────────────────
    public string       Firma        { get; set; } = string.Empty;
    public string       Model        { get; set; } = string.Empty;
    public int          AnFabricatie { get; set; }
    public string       Culoare      { get; set; } = string.Empty;
    public List<string> Optiuni      { get; set; } = new();

    // ─── Detalii tranzacție ─────────────────────────────────────────────────
    public DateTime DataTranzactie { get; set; }
    public decimal  Pret           { get; set; }

    // ─── Constructor ────────────────────────────────────────────────────────
    public TranzactieAuto() { }

    public TranzactieAuto(
        int          id,
        string       numeVanzator,
        string       numeCumparator,
        string       firma,
        string       model,
        int          anFabricatie,
        string       culoare,
        List<string> optiuni,
        DateTime     dataTranzactie,
        decimal      pret)
    {
        Id              = id;
        NumeVanzator    = numeVanzator;
        NumeCumparator  = numeCumparator;
        Firma           = firma;
        Model           = model;
        AnFabricatie    = anFabricatie;
        Culoare         = culoare;
        Optiuni         = optiuni;
        DataTranzactie  = dataTranzactie;
        Pret            = pret;
    }

    // ─── Reprezentare text ──────────────────────────────────────────────────
    public override string ToString()
    {
        string optiuniText = Optiuni.Count > 0
            ? string.Join(", ", Optiuni)
            : "Fără opțiuni suplimentare";

        return $"""
                ┌─────────────────────────────────────────────────
                │  Tranzacție #{Id:D4}
                │  Data       : {DataTranzactie:dd/MM/yyyy}
                ├─────────────────────────────────────────────────
                │  Vânzător   : {NumeVanzator}
                │  Cumpărător : {NumeCumparator}
                ├─────────────────────────────────────────────────
                │  Vehicul    : {Firma} {Model} ({AnFabricatie})
                │  Culoare    : {Culoare}
                │  Opțiuni    : {optiuniText}
                ├─────────────────────────────────────────────────
                │  Preț       : {Pret:C2}
                └─────────────────────────────────────────────────
                """;
    }
}
