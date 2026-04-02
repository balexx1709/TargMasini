using System.Globalization;
using TargMasini.Entitati;
using TargMasini.Entitati.Enums;

namespace TargMasini.Logica.StocareDate;

/// <summary>
/// Strat de stocare în fișier text pentru TranzactieAuto.
/// Cerință temă 7: nivelul StocareDate cu salvare în fișier text.
///
/// Format linie (separator |):
///   id|numeVanzator|numeCumparator|firma|model|anFabricatie|culoare|optiuni|dataTranzactie|pret
///
/// Exemplu:
///   1|Ion Popescu|Maria Ionescu|BMW|Seria 5|2021|Negru|56|15/03/2024|45000.00
///   (optiuni=56 = Trapa(8)+ScauneIncalzite(16)+Navigatie(2)+Xenon(32))
/// </summary>
public class FisierTranzactii
{
    private readonly string _caleFisier;
    private const char SEPARATOR = '|';
    private const string FORMAT_DATA = "dd/MM/yyyy";

    public FisierTranzactii(string caleFisier = "tranzactii.txt")
    {
        _caleFisier = caleFisier;
    }

    // ─── SALVARE ──────────────────────────────────────────────────────────────

    /// <summary>Salvează toate tranzacțiile în fișier (suprascrie).</summary>
    public void SalveazaToate(IEnumerable<TranzactieAuto> tranzactii)
    {
        var linii = tranzactii.Select(SerializeazaTranzactie);
        File.WriteAllLines(_caleFisier, linii);
    }

    /// <summary>Adaugă o tranzacție la sfârșitul fișierului.</summary>
    public void AdaugaInFisier(TranzactieAuto t)
    {
        File.AppendAllText(_caleFisier, SerializeazaTranzactie(t) + Environment.NewLine);
    }

    // ─── ÎNCĂRCARE ────────────────────────────────────────────────────────────

    /// <summary>Încarcă toate tranzacțiile din fișier.</summary>
    public List<TranzactieAuto> IncarcaToate()
    {
        if (!File.Exists(_caleFisier)) return new List<TranzactieAuto>();

        return File.ReadAllLines(_caleFisier)
            .Where(linie => !string.IsNullOrWhiteSpace(linie))
            .Select(DeserializeazaTranzactie)
            .Where(t => t is not null)
            .Cast<TranzactieAuto>()
            .ToList();
    }

    // ─── CĂUTARE ÎN FIȘIER (LINQ pe date încărcate) ──────────────────────────

    /// <summary>Caută tranzacții după firmă, direct din fișier (LINQ).</summary>
    public List<TranzactieAuto> CautaDupaFirma(string firma) =>
        IncarcaToate()
            .Where(t => t.Firma.Equals(firma, StringComparison.OrdinalIgnoreCase))
            .ToList();

    /// <summary>Caută tranzacții după vânzător, direct din fișier (LINQ).</summary>
    public List<TranzactieAuto> CautaDupaVanzator(string numePartial) =>
        IncarcaToate()
            .Where(t => t.NumeVanzator.Contains(numePartial, StringComparison.OrdinalIgnoreCase))
            .ToList();

    /// <summary>Caută tranzacții cu preț într-un interval, direct din fișier (LINQ).</summary>
    public List<TranzactieAuto> CautaDupaInterval(decimal pretMin, decimal pretMax) =>
        IncarcaToate()
            .Where(t => t.Pret >= pretMin && t.Pret <= pretMax)
            .ToList();

    // ─── MODIFICARE ──────────────────────────────────────────────────────────

    /// <summary>
    /// Modifică prețul unei tranzacții în fișier.
    /// Reîncarcă tot, modifică, salvează tot (pattern standard pentru fișiere text).
    /// </summary>
    public bool ModificaPret(int id, decimal pretNou)
    {
        var toate = IncarcaToate();
        var t = toate.FirstOrDefault(x => x.Id == id);
        if (t is null) return false;
        t.Pret = pretNou;
        SalveazaToate(toate);
        return true;
    }

    /// <summary>Șterge o tranzacție din fișier după ID.</summary>
    public bool StergeDinFisier(int id)
    {
        var toate = IncarcaToate();
        var initCount = toate.Count;
        toate.RemoveAll(t => t.Id == id);
        if (toate.Count == initCount) return false;
        SalveazaToate(toate);
        return true;
    }

    // ─── SERIALIZARE / DESERIALIZARE ──────────────────────────────────────────

    private string SerializeazaTranzactie(TranzactieAuto t) =>
        string.Join(SEPARATOR,
            t.Id,
            t.NumeVanzator,
            t.NumeCumparator,
            t.Firma,
            t.Model,
            t.AnFabricatie,
            (int)t.Culoare,
            (int)t.Optiuni,
            t.DataTranzactie.ToString(FORMAT_DATA, CultureInfo.InvariantCulture),
            t.Pret.ToString("F2", CultureInfo.InvariantCulture));

    private TranzactieAuto? DeserializeazaTranzactie(string linie)
    {
        try
        {
            var p = linie.Split(SEPARATOR);
            if (p.Length < 10) return null;

            return new TranzactieAuto(
                id:              int.Parse(p[0]),
                numeVanzator:    p[1],
                numeCumparator:  p[2],
                firma:           p[3],
                model:           p[4],
                anFabricatie:    int.Parse(p[5]),
                culoare:         (CuloareAuto)int.Parse(p[6]),
                optiuni:         (OptiuniAuto)int.Parse(p[7]),
                dataTranzactie:  DateTime.ParseExact(p[8], FORMAT_DATA, CultureInfo.InvariantCulture),
                pret:            decimal.Parse(p[9], CultureInfo.InvariantCulture));
        }
        catch
        {
            return null; // linie coruptă — ignorăm
        }
    }
}
