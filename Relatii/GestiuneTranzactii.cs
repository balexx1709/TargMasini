using TargMasini.Entitati;

namespace TargMasini.Relatii;

/// <summary>
/// Gestionează lista de tranzacții și operațiile CRUD.
/// Echivalent cu "relația" dintre entitățile sistemului.
/// </summary>
public class GestiuneTranzactii
{
    private readonly List<TranzactieAuto> _tranzactii = new();
    private int _nextId = 1;

    // ════════════════════════════════════════════════════════════════════════
    //  CRUD
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>Adaugă o tranzacție nouă.</summary>
    public TranzactieAuto Adauga(
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
        var t = new TranzactieAuto(
            _nextId++, numeVanzator, numeCumparator,
            firma, model, anFabricatie, culoare,
            optiuni, dataTranzactie, pret);

        _tranzactii.Add(t);
        return t;
    }

    /// <summary>Adaugă direct un obiect TranzactieAuto.</summary>
    public void AdaugaObiect(TranzactieAuto t)
    {
        t.Id = _nextId++;
        _tranzactii.Add(t);
    }

    /// <summary>Returnează toate tranzacțiile.</summary>
    public IReadOnlyList<TranzactieAuto> GetToate() => _tranzactii.AsReadOnly();

    /// <summary>Caută după ID.</summary>
    public TranzactieAuto? GasesteDupaId(int id) =>
        _tranzactii.FirstOrDefault(t => t.Id == id);

    /// <summary>Șterge o tranzacție după ID.</summary>
    public bool Sterge(int id)
    {
        var t = GasesteDupaId(id);
        if (t is null) return false;
        _tranzactii.Remove(t);
        return true;
    }

    // ════════════════════════════════════════════════════════════════════════
    //  INTEROGĂRI LINQ – Filtrări
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>Filtrează tranzacțiile după firmă (case-insensitive).</summary>
    public IEnumerable<TranzactieAuto> DupaFirma(string firma) =>
        _tranzactii.Where(t =>
            t.Firma.Equals(firma, StringComparison.OrdinalIgnoreCase));

    /// <summary>Filtrează tranzacțiile după vânzător (parțial).</summary>
    public IEnumerable<TranzactieAuto> DupaVanzator(string numePartial) =>
        _tranzactii.Where(t =>
            t.NumeVanzator.Contains(numePartial, StringComparison.OrdinalIgnoreCase));

    /// <summary>Filtrează tranzacțiile după cumpărător (parțial).</summary>
    public IEnumerable<TranzactieAuto> DupaCumparator(string numePartial) =>
        _tranzactii.Where(t =>
            t.NumeCumparator.Contains(numePartial, StringComparison.OrdinalIgnoreCase));

    /// <summary>Filtrează tranzacțiile dintr-un interval de prețuri.</summary>
    public IEnumerable<TranzactieAuto> DupaInterval(decimal pretMin, decimal pretMax) =>
        _tranzactii.Where(t => t.Pret >= pretMin && t.Pret <= pretMax);

    /// <summary>Filtrează tranzacțiile dintr-un interval de ani de fabricație.</summary>
    public IEnumerable<TranzactieAuto> DupaAnFabricatie(int anMin, int anMax) =>
        _tranzactii.Where(t => t.AnFabricatie >= anMin && t.AnFabricatie <= anMax);

    /// <summary>Filtrează tranzacțiile dintr-un interval de date.</summary>
    public IEnumerable<TranzactieAuto> DupaPerioada(DateTime de, DateTime panaLa) =>
        _tranzactii.Where(t => t.DataTranzactie >= de && t.DataTranzactie <= panaLa);

    /// <summary>Filtrează tranzacțiile ce conțin o anumită opțiune.</summary>
    public IEnumerable<TranzactieAuto> CuOptiune(string optiune) =>
        _tranzactii.Where(t =>
            t.Optiuni.Any(o => o.Equals(optiune, StringComparison.OrdinalIgnoreCase)));

    // ════════════════════════════════════════════════════════════════════════
    //  INTEROGĂRI LINQ – Sortări
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>Sortează tranzacțiile după preț ascendent.</summary>
    public IEnumerable<TranzactieAuto> SortateDupaPrețAsc() =>
        _tranzactii.OrderBy(t => t.Pret);

    /// <summary>Sortează tranzacțiile după preț descendent.</summary>
    public IEnumerable<TranzactieAuto> SortateDupaPrețDesc() =>
        _tranzactii.OrderByDescending(t => t.Pret);

    /// <summary>Sortează tranzacțiile după data tranzacției (cele mai recente primele).</summary>
    public IEnumerable<TranzactieAuto> SortateDupaDataDesc() =>
        _tranzactii.OrderByDescending(t => t.DataTranzactie);

    // ════════════════════════════════════════════════════════════════════════
    //  INTEROGĂRI LINQ – Statistici
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>Valoarea totală a tuturor tranzacțiilor.</summary>
    public decimal ValoareTotala() => _tranzactii.Sum(t => t.Pret);

    /// <summary>Prețul mediu al tranzacțiilor.</summary>
    public decimal PretMediu() =>
        _tranzactii.Count > 0 ? _tranzactii.Average(t => t.Pret) : 0;

    /// <summary>Tranzacția cu cel mai mare preț.</summary>
    public TranzactieAuto? CelMaiScump() =>
        _tranzactii.MaxBy(t => t.Pret);

    /// <summary>Tranzacția cu cel mai mic preț.</summary>
    public TranzactieAuto? CelMaiIeftin() =>
        _tranzactii.MinBy(t => t.Pret);

    /// <summary>Numărul de tranzacții grupate pe firmă.</summary>
    public IEnumerable<(string Firma, int Numar)> GrupariPeFirma() =>
        _tranzactii
            .GroupBy(t => t.Firma)
            .Select(g => (Firma: g.Key, Numar: g.Count()))
            .OrderByDescending(x => x.Numar);

    /// <summary>Suma tranzacțiilor grupată pe vânzător.</summary>
    public IEnumerable<(string Vanzator, decimal Total)> TotalPeVanzator() =>
        _tranzactii
            .GroupBy(t => t.NumeVanzator)
            .Select(g => (Vanzator: g.Key, Total: g.Sum(t => t.Pret)))
            .OrderByDescending(x => x.Total);

    /// <summary>Cele mai populare opțiuni după frecvență.</summary>
    public IEnumerable<(string Optiune, int Frecventa)> OptiuniPopulare() =>
        _tranzactii
            .SelectMany(t => t.Optiuni)
            .GroupBy(o => o)
            .Select(g => (Optiune: g.Key, Frecventa: g.Count()))
            .OrderByDescending(x => x.Frecventa);

    /// <summary>Distribuția tranzacțiilor pe ani de fabricație.</summary>
    public IEnumerable<(int An, int Numar, decimal ValoareMedie)> DistributieAnFabricatie() =>
        _tranzactii
            .GroupBy(t => t.AnFabricatie)
            .Select(g => (An: g.Key, Numar: g.Count(), ValoareMedie: g.Average(t => t.Pret)))
            .OrderByDescending(x => x.An);
}
