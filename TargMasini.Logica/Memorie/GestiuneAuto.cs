using TargMasini.Entitati;
using TargMasini.Entitati.Enums;

namespace TargMasini.Logica.Memorie;

/// <summary>
/// Administrator în memorie pentru a doua entitate: Auto.
/// Cerință temă 7: implementare facilități pentru a doua entitate.
/// </summary>
public class GestiuneAuto
{
    private readonly List<Auto> _masini = new();
    private int _nextId = 1;

    // ─── CRUD ────────────────────────────────────────────────────────────────

    public Auto Adauga(string firma, string model, int anFabricatie,
                       CuloareAuto culoare, OptiuniAuto optiuni, decimal pret,
                       string descriere = "")
    {
        var a = new Auto(_nextId++, firma, model, anFabricatie,
                         culoare, optiuni, pret, descriere);
        _masini.Add(a);
        return a;
    }

    public void AdaugaObiect(Auto a)
    {
        if (a.Id == 0) a.Id = _nextId;
        if (a.Id >= _nextId) _nextId = a.Id + 1;
        _masini.Add(a);
    }

    public IReadOnlyList<Auto> GetToate() => _masini.AsReadOnly();

    public Auto? GasesteDupaId(int id) => _masini.FirstOrDefault(a => a.Id == id);

    public bool Sterge(int id)
    {
        var a = GasesteDupaId(id);
        if (a is null) return false;
        _masini.Remove(a);
        return true;
    }

    public bool MarcheazaVanduta(int id)
    {
        var a = GasesteDupaId(id);
        if (a is null) return false;
        a.EsteVanduta = true;
        return true;
    }

    // ─── LINQ – Filtrări ──────────────────────────────────────────────────────

    public IEnumerable<Auto> Disponibile() =>
        _masini.Where(a => !a.EsteVanduta);

    public IEnumerable<Auto> DupaFirma(string firma) =>
        _masini.Where(a => a.Firma.Equals(firma, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<Auto> DupaCuloare(CuloareAuto culoare) =>
        _masini.Where(a => a.Culoare == culoare);

    public IEnumerable<Auto> CuOptiune(OptiuniAuto optiune) =>
        _masini.Where(a => a.Optiuni.HasFlag(optiune));

    public IEnumerable<Auto> DupaInterval(decimal pretMin, decimal pretMax) =>
        _masini.Where(a => a.Pret >= pretMin && a.Pret <= pretMax);

    public IEnumerable<Auto> DupaAnFabricatie(int anMin, int anMax) =>
        _masini.Where(a => a.AnFabricatie >= anMin && a.AnFabricatie <= anMax);

    // ─── LINQ – Statistici ───────────────────────────────────────────────────

    public decimal PretMediuDisponibil() =>
        _masini.Where(a => !a.EsteVanduta).DefaultIfEmpty().Average(a => a?.Pret ?? 0);

    public IEnumerable<(string Firma, int Numar)> GrupariPeFirma() =>
        _masini.GroupBy(a => a.Firma)
               .Select(g => (Firma: g.Key, Numar: g.Count()))
               .OrderByDescending(x => x.Numar);
}
