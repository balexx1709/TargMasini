using TargMasini.Entitati.Enums;

namespace TargMasini.Entitati;

/// <summary>
/// Entitate principală: reprezintă o tranzacție de vânzare/cumpărare auto.
/// CuloareAuto și OptiuniAuto sunt câmpuri de tip enum (cerință temă).
/// </summary>
public class TranzactieAuto
{
    // ─── Identificare ───────────────────────────────────────────────────────
    public int Id { get; set; }

    // ─── Participanți ───────────────────────────────────────────────────────
    public string NumeVanzator   { get; set; } = string.Empty;
    public string NumeCumparator { get; set; } = string.Empty;

    // ─── Detalii vehicul ────────────────────────────────────────────────────
    public string       Firma        { get; set; } = string.Empty;
    public string       Model        { get; set; } = string.Empty;
    public int          AnFabricatie { get; set; }

    /// <summary>Culoarea vehiculului — câmp de tip enum.</summary>
    public CuloareAuto  Culoare      { get; set; } = CuloareAuto.Negru;

    /// <summary>
    /// Opțiunile vehiculului — enum cu [Flags], permite combinare cu |.
    /// Ex: OptiuniAuto.Trapa | OptiuniAuto.Navigatie
    /// </summary>
    public OptiuniAuto  Optiuni      { get; set; } = OptiuniAuto.Niciuna;

    // ─── Detalii tranzacție ─────────────────────────────────────────────────
    public DateTime DataTranzactie { get; set; }
    public decimal  Pret           { get; set; }

    // ─── Constructori ────────────────────────────────────────────────────────
    public TranzactieAuto() { }

    public TranzactieAuto(
        int        id,
        string     numeVanzator,
        string     numeCumparator,
        string     firma,
        string     model,
        int        anFabricatie,
        CuloareAuto culoare,
        OptiuniAuto optiuni,
        DateTime   dataTranzactie,
        decimal    pret)
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

    // ─── Helper: afișează opțiunile active din flags ─────────────────────────
    public string OptiuniText()
    {
        if (Optiuni == OptiuniAuto.Niciuna)
            return "Fără opțiuni suplimentare";

        return string.Join(", ",
            Enum.GetValues<OptiuniAuto>()
                .Where(o => o != OptiuniAuto.Niciuna && Optiuni.HasFlag(o)));
    }

    public override string ToString() =>
        $"""
         ┌─────────────────────────────────────────────────
         │  Tranzacție #{Id:D4}
         │  Data       : {DataTranzactie:dd/MM/yyyy}
         ├─────────────────────────────────────────────────
         │  Vânzător   : {NumeVanzator}
         │  Cumpărător : {NumeCumparator}
         ├─────────────────────────────────────────────────
         │  Vehicul    : {Firma} {Model} ({AnFabricatie})
         │  Culoare    : {Culoare}
         │  Opțiuni    : {OptiuniText()}
         ├─────────────────────────────────────────────────
         │  Preț       : {Pret:C2}
         └─────────────────────────────────────────────────
         """;
}
