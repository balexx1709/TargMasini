using TargMasini.Entitati;
using TargMasini.Entitati.Enums;

namespace TargMasini.Logica.Memorie;

/// <summary>
/// Administrator în memorie pentru TranzactieAuto.
/// Echivalent cu "AdministratorEntitateMemorie" din modelul laborator.
/// Toate metodele de căutare/filtrare utilizează LINQ (cerință temă).
/// </summary>
public class GestiuneTranzactii
{
    private readonly List<TranzactieAuto> _tranzactii = new();
    private int _nextId = 1;

    // ════════════════════════════════════════════════════════════════════════
    //  CRUD – Citire de la tastatură / Salvare / Afișare (cerință temă lab anterior)
    // ════════════════════════════════════════════════════════════════════════

    public TranzactieAuto Adauga(
        string      numeVanzator,
        string      numeCumparator,
        string      firma,
        string      model,
        int         anFabricatie,
        CuloareAuto culoare,
        OptiuniAuto optiuni,
        DateTime    dataTranzactie,
        decimal     pret)
    {
        var t = new TranzactieAuto(
            _nextId++, numeVanzator, numeCumparator,
            firma, model, anFabricatie, culoare, optiuni,
            dataTranzactie, pret);
        _tranzactii.Add(t);
        return t;
    }

    public void AdaugaObiect(TranzactieAuto t)
    {
        if (t.Id == 0) t.Id = _nextId;
        if (t.Id >= _nextId) _nextId = t.Id + 1;
        _tranzactii.Add(t);
    }

    public IReadOnlyList<TranzactieAuto> GetToate() => _tranzactii.AsReadOnly();

    public TranzactieAuto? GasesteDupaId(int id) =>
        _tranzactii.FirstOrDefault(t => t.Id == id);

    public bool Sterge(int id)
    {
        var t = GasesteDupaId(id);
        if (t is null) return false;
        _tranzactii.Remove(t);
        return true;
    }

    // ════════════════════════════════════════════════════════════════════════
    //  LINQ – Filtrări (cerință temă: cel puțin o metodă cu LINQ)
    // ════════════════════════════════════════════════════════════════════════

    /// <summary>Filtrare după firmă (LINQ Where).</summary>
    public IEnumerable<TranzactieAuto> DupaFirma(string firma) =>
        _tranzactii.Where(t => t.Firma.Equals(firma, StringComparison.OrdinalIgnoreCase));

    /// <summary>Filtrare după vânzător parțial (LINQ Where + Contains).</summary>
    public IEnumerable<TranzactieAuto> DupaVanzator(string numePartial) =>
        _tranzactii.Where(t => t.NumeVanzator.Contains(numePartial, StringComparison.OrdinalIgnoreCase));

    /// <summary>Filtrare după cumpărător parțial (LINQ Where + Contains).</summary>
    public IEnumerable<TranzactieAuto> DupaCumparator(string numePartial) =>
        _tranzactii.Where(t => t.NumeCumparator.Contains(numePartial, StringComparison.OrdinalIgnoreCase));

    /// <summary>Filtrare după interval de prețuri (LINQ Where).</summary>
    public IEnumerable<TranzactieAuto> DupaInterval(decimal pretMin, decimal pretMax) =>
        _tranzactii.Where(t => t.Pret >= pretMin && t.Pret <= pretMax);

    /// <summary>Filtrare după interval de ani (LINQ Where).</summary>
    public IEnumerable<TranzactieAuto> DupaAnFabricatie(int anMin, int anMax) =>
        _tranzactii.Where(t => t.AnFabricatie >= anMin && t.AnFabricatie <= anMax);

    /// <summary>Filtrare după perioadă (LINQ Where).</summary>
    public IEnumerable<TranzactieAuto> DupaPerioada(DateTime de, DateTime panaLa) =>
        _tranzactii.Where(t => t.DataTranzactie >= de && t.DataTranzactie <= panaLa);

    /// <summary>
    /// Filtrare după culoare (LINQ Where pe enum CuloareAuto).
    /// O singură comparație simplă — posibilă datorită enum-ului.
    /// </summary>
    public IEnumerable<TranzactieAuto> DupaCuloare(CuloareAuto culoare) =>
        _tranzactii.Where(t => t.Culoare == culoare);

    /// <summary>
    /// Filtrare după opțiune (LINQ Where cu HasFlag pe [Flags] enum).
    /// HasFlag() verifică dacă bitul corespunzător opțiunii este setat.
    /// </summary>
    public IEnumerable<TranzactieAuto> CuOptiune(OptiuniAuto optiune) =>
        _tranzactii.Where(t => t.Optiuni.HasFlag(optiune));

    // ════════════════════════════════════════════════════════════════════════
    //  LINQ – Sortări
    // ════════════════════════════════════════════════════════════════════════

    public IEnumerable<TranzactieAuto> SortateDupaPrețAsc()  =>
        _tranzactii.OrderBy(t => t.Pret);

    public IEnumerable<TranzactieAuto> SortateDupaPrețDesc() =>
        _tranzactii.OrderByDescending(t => t.Pret);

    public IEnumerable<TranzactieAuto> SortateDupaDataDesc() =>
        _tranzactii.OrderByDescending(t => t.DataTranzactie);

    // ════════════════════════════════════════════════════════════════════════
    //  LINQ – Statistici (GroupBy, Sum, Average, Max, Min)
    // ════════════════════════════════════════════════════════════════════════

    public decimal ValoareTotala() => _tranzactii.Sum(t => t.Pret);

    public decimal PretMediu() =>
        _tranzactii.Count > 0 ? _tranzactii.Average(t => t.Pret) : 0;

    public TranzactieAuto? CelMaiScump()  => _tranzactii.MaxBy(t => t.Pret);
    public TranzactieAuto? CelMaiIeftin() => _tranzactii.MinBy(t => t.Pret);

    /// <summary>GroupBy firmă + Count (LINQ GroupBy).</summary>
    public IEnumerable<(string Firma, int Numar, decimal Total)> GrupariPeFirma() =>
        _tranzactii
            .GroupBy(t => t.Firma)
            .Select(g => (Firma: g.Key, Numar: g.Count(), Total: g.Sum(t => t.Pret)))
            .OrderByDescending(x => x.Total);

    /// <summary>GroupBy vânzător + Sum (LINQ GroupBy + Sum).</summary>
    public IEnumerable<(string Vanzator, int Numar, decimal Total)> TotalPeVanzator() =>
        _tranzactii
            .GroupBy(t => t.NumeVanzator)
            .Select(g => (Vanzator: g.Key, Numar: g.Count(), Total: g.Sum(t => t.Pret)))
            .OrderByDescending(x => x.Total);

    /// <summary>
    /// Opțiunile cele mai frecvente (LINQ SelectMany pe flags enum).
    /// Despachetăm fiecare bit activ și numărăm frecvența.
    /// </summary>
    public IEnumerable<(OptiuniAuto Optiune, int Frecventa)> OptiuniPopulare() =>
        _tranzactii
            .SelectMany(t => Enum.GetValues<OptiuniAuto>()
                .Where(o => o != OptiuniAuto.Niciuna && t.Optiuni.HasFlag(o)))
            .GroupBy(o => o)
            .Select(g => (Optiune: g.Key, Frecventa: g.Count()))
            .OrderByDescending(x => x.Frecventa);

    /// <summary>Distribuție pe an + medie preț (LINQ GroupBy + Average).</summary>
    public IEnumerable<(int An, int Numar, decimal ValoareMedie)> DistributieAnFabricatie() =>
        _tranzactii
            .GroupBy(t => t.AnFabricatie)
            .Select(g => (An: g.Key, Numar: g.Count(), ValoareMedie: g.Average(t => t.Pret)))
            .OrderByDescending(x => x.An);

    /// <summary>Distribuție pe culoare (LINQ GroupBy pe enum CuloareAuto).</summary>
    public IEnumerable<(CuloareAuto Culoare, int Numar)> DistributieCulori() =>
        _tranzactii
            .GroupBy(t => t.Culoare)
            .Select(g => (Culoare: g.Key, Numar: g.Count()))
            .OrderByDescending(x => x.Numar);
}
