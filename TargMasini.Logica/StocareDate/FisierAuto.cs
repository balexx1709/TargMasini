using System.Globalization;
using TargMasini.Entitati;
using TargMasini.Entitati.Enums;

namespace TargMasini.Logica.StocareDate;

/// <summary>
/// Strat de stocare în fișier text pentru entitatea Auto (a doua entitate).
/// Cerință temă 7, pct. 3: facilități pentru a doua entitate.
///
/// Format linie (separator |):
///   id|firma|model|anFabricatie|culoare|optiuni|pret|esteVanduta|descriere
/// </summary>
public class FisierAuto
{
    private readonly string _caleFisier;
    private const char SEPARATOR = '|';
    private const string FORMAT_BOOL_DA = "da";
    private const string FORMAT_BOOL_NU = "nu";

    public FisierAuto(string caleFisier = "masini.txt")
    {
        _caleFisier = caleFisier;
    }

    // ─── SALVARE ──────────────────────────────────────────────────────────────

    public void SalveazaToate(IEnumerable<Auto> masini)
    {
        var linii = masini.Select(SerializeazaAuto);
        File.WriteAllLines(_caleFisier, linii);
    }

    public void AdaugaInFisier(Auto a)
    {
        File.AppendAllText(_caleFisier, SerializeazaAuto(a) + Environment.NewLine);
    }

    // ─── ÎNCĂRCARE ────────────────────────────────────────────────────────────

    public List<Auto> IncarcaToate()
    {
        if (!File.Exists(_caleFisier)) return new List<Auto>();

        return File.ReadAllLines(_caleFisier)
            .Where(linie => !string.IsNullOrWhiteSpace(linie))
            .Select(DeserializeazaAuto)
            .Where(a => a is not null)
            .Cast<Auto>()
            .ToList();
    }

    // ─── CĂUTARE (LINQ pe date încărcate) ────────────────────────────────────

    public List<Auto> CautaDupaFirma(string firma) =>
        IncarcaToate()
            .Where(a => a.Firma.Equals(firma, StringComparison.OrdinalIgnoreCase))
            .ToList();

    public List<Auto> CautaDisponibile() =>
        IncarcaToate().Where(a => !a.EsteVanduta).ToList();

    public List<Auto> CautaCuOptiune(OptiuniAuto optiune) =>
        IncarcaToate().Where(a => a.Optiuni.HasFlag(optiune)).ToList();

    // ─── MODIFICARE ──────────────────────────────────────────────────────────

    public bool ModificaPret(int id, decimal pretNou)
    {
        var toate = IncarcaToate();
        var a = toate.FirstOrDefault(x => x.Id == id);
        if (a is null) return false;
        a.Pret = pretNou;
        SalveazaToate(toate);
        return true;
    }

    public bool MarcheazaVanduta(int id)
    {
        var toate = IncarcaToate();
        var a = toate.FirstOrDefault(x => x.Id == id);
        if (a is null) return false;
        a.EsteVanduta = true;
        SalveazaToate(toate);
        return true;
    }

    public bool StergeDinFisier(int id)
    {
        var toate = IncarcaToate();
        var initCount = toate.Count;
        toate.RemoveAll(a => a.Id == id);
        if (toate.Count == initCount) return false;
        SalveazaToate(toate);
        return true;
    }

    // ─── SERIALIZARE / DESERIALIZARE ──────────────────────────────────────────

    private string SerializeazaAuto(Auto a) =>
        string.Join(SEPARATOR,
            a.Id, a.Firma, a.Model, a.AnFabricatie,
            (int)a.Culoare, (int)a.Optiuni,
            a.Pret.ToString("F2", CultureInfo.InvariantCulture),
            a.EsteVanduta ? FORMAT_BOOL_DA : FORMAT_BOOL_NU,
            a.Descriere.Replace("|", " "));

    private Auto? DeserializeazaAuto(string linie)
    {
        try
        {
            var p = linie.Split(SEPARATOR);
            if (p.Length < 8) return null;

            return new Auto(
                id:           int.Parse(p[0]),
                firma:        p[1],
                model:        p[2],
                anFabricatie: int.Parse(p[3]),
                culoare:      (CuloareAuto)int.Parse(p[4]),
                optiuni:      (OptiuniAuto)int.Parse(p[5]),
                pret:         decimal.Parse(p[6], CultureInfo.InvariantCulture),
                descriere:    p.Length > 8 ? p[8] : "")
            {
                EsteVanduta = p[7].Equals(FORMAT_BOOL_DA, StringComparison.OrdinalIgnoreCase)
            };
        }
        catch
        {
            return null;
        }
    }
}
