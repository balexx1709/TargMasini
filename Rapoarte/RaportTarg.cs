using TargMasini.Entitati;
using TargMasini.Relatii;

namespace TargMasini.Rapoarte;

/// <summary>
/// Generează rapoarte formatate pe baza datelor din GestiuneTranzactii.
/// Echivalentul stratului de "Raport" din arhitectura Entitate->Relație->Raport.
/// </summary>
public class RaportTarg
{
    private readonly GestiuneTranzactii _gestiune;

    public RaportTarg(GestiuneTranzactii gestiune)
    {
        _gestiune = gestiune;
    }

    // ════════════════════════════════════════════════════════════════════════
    //  RAPORT GENERAL
    // ════════════════════════════════════════════════════════════════════════

    public void AfiseazaRaportGeneral()
    {
        var toate = _gestiune.GetToate();

        Titlu("RAPORT GENERAL - TÂRG DE MAȘINI");
        Console.WriteLine($"  Data raportului : {DateTime.Now:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"  Total tranzacții: {toate.Count}");
        Console.WriteLine($"  Valoare totală  : {_gestiune.ValoareTotala():C2}");
        Console.WriteLine($"  Preț mediu      : {_gestiune.PretMediu():C2}");

        var maxT = _gestiune.CelMaiScump();
        var minT = _gestiune.CelMaiIeftin();

        if (maxT is not null)
            Console.WriteLine($"  Cel mai scump   : {maxT.Firma} {maxT.Model} - {maxT.Pret:C2}");
        if (minT is not null)
            Console.WriteLine($"  Cel mai ieftin  : {minT.Firma} {minT.Model} - {minT.Pret:C2}");

        Separator();
    }

    // ════════════════════════════════════════════════════════════════════════
    //  RAPORT TRANZACȚII
    // ════════════════════════════════════════════════════════════════════════

    public void AfiseazaToateTranzactiile()
    {
        Titlu("TOATE TRANZACȚIILE");
        foreach (var t in _gestiune.SortateDupaDataDesc())
            Console.WriteLine(t);
        Separator();
    }

    public void AfiseazaTranzactiiFirma(string firma)
    {
        var lista = _gestiune.DupaFirma(firma).ToList();
        Titlu($"TRANZACȚII PENTRU FIRMA: {firma.ToUpper()}");
        Console.WriteLine($"  Găsite: {lista.Count} tranzacții\n");
        foreach (var t in lista.OrderByDescending(t => t.DataTranzactie))
            Console.WriteLine(t);
        Separator();
    }

    public void AfiseazaTranzactiiInterval(decimal pretMin, decimal pretMax)
    {
        var lista = _gestiune.DupaInterval(pretMin, pretMax).ToList();
        Titlu($"TRANZACȚII ÎN INTERVALUL: {pretMin:C2} – {pretMax:C2}");
        Console.WriteLine($"  Găsite: {lista.Count} tranzacții\n");
        foreach (var t in lista.OrderBy(t => t.Pret))
            Console.WriteLine(t);
        Separator();
    }

    public void AfiseazaTranzactiiPerioada(DateTime de, DateTime panaLa)
    {
        var lista = _gestiune.DupaPerioada(de, panaLa).ToList();
        Titlu($"TRANZACȚII PERIOADĂ: {de:dd/MM/yyyy} – {panaLa:dd/MM/yyyy}");
        Console.WriteLine($"  Găsite: {lista.Count} tranzacții\n");
        foreach (var t in lista.OrderByDescending(t => t.DataTranzactie))
            Console.WriteLine(t);
        Separator();
    }

    // ════════════════════════════════════════════════════════════════════════
    //  RAPORT STATISTICI
    // ════════════════════════════════════════════════════════════════════════

    public void AfiseazaStatisticiPeFirma()
    {
        Titlu("STATISTICI PE FIRMĂ");
        Console.WriteLine($"  {"Firmă",-20} {"Nr. Tranzacții",15} {"Valoare Totală",18}");
        Console.WriteLine(new string('─', 56));

        var grupe = _gestiune.GetToate()
            .GroupBy(t => t.Firma)
            .Select(g => new
            {
                Firma   = g.Key,
                Numar   = g.Count(),
                Total   = g.Sum(t => t.Pret),
                Medie   = g.Average(t => t.Pret)
            })
            .OrderByDescending(x => x.Total);

        foreach (var g in grupe)
        {
            Console.WriteLine($"  {g.Firma,-20} {g.Numar,15} {g.Total,18:C2}");
            Console.WriteLine($"  {"  Preț mediu:",-20} {"",-15} {g.Medie,18:C2}");
        }
        Separator();
    }

    public void AfiseazaTopVanzatori()
    {
        Titlu("TOP VÂNZĂTORI (după valoare)");
        Console.WriteLine($"  {"#",-4} {"Vânzător",-25} {"Tranzacții",12} {"Valoare Totală",18}");
        Console.WriteLine(new string('─', 62));

        var top = _gestiune.GetToate()
            .GroupBy(t => t.NumeVanzator)
            .Select(g => new
            {
                Vanzator = g.Key,
                Numar    = g.Count(),
                Total    = g.Sum(t => t.Pret)
            })
            .OrderByDescending(x => x.Total)
            .ToList();

        for (int i = 0; i < top.Count; i++)
        {
            var v = top[i];
            Console.WriteLine($"  {i + 1,-4} {v.Vanzator,-25} {v.Numar,12} {v.Total,18:C2}");
        }
        Separator();
    }

    public void AfiseazaOptiuniPopulare()
    {
        Titlu("OPȚIUNI CELE MAI SOLICITATE");
        Console.WriteLine($"  {"Opțiune",-35} {"Frecvență",12} {"% din total",14}");
        Console.WriteLine(new string('─', 64));

        var totalTranzactii = _gestiune.GetToate().Count;
        var optiuni = _gestiune.OptiuniPopulare().ToList();

        foreach (var (optiune, frecventa) in optiuni)
        {
            double procent = totalTranzactii > 0
                ? (double)frecventa / totalTranzactii * 100
                : 0;
            Console.WriteLine($"  {optiune,-35} {frecventa,12} {procent,13:F1}%");
        }
        Separator();
    }

    public void AfiseazaDistributieAnFabricatie()
    {
        Titlu("DISTRIBUȚIE PE AN DE FABRICAȚIE");
        Console.WriteLine($"  {"An",-8} {"Nr. Mașini",12} {"Preț Mediu",18} {"Bar"}");
        Console.WriteLine(new string('─', 72));

        var dist = _gestiune.DistributieAnFabricatie().ToList();
        int maxNumar = dist.Count > 0 ? dist.Max(d => d.Numar) : 1;

        foreach (var (an, numar, valMedie) in dist)
        {
            int barLength = (int)Math.Round((double)numar / maxNumar * 20);
            string bar = new string('█', barLength);
            Console.WriteLine($"  {an,-8} {numar,12} {valMedie,18:C2} {bar}");
        }
        Separator();
    }

    public void AfiseazaRaportComplet()
    {
        try { Console.Clear(); } catch { }
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        AfiseazaRaportGeneral();
        AfiseazaStatisticiPeFirma();
        AfiseazaTopVanzatori();
        AfiseazaOptiuniPopulare();
        AfiseazaDistributieAnFabricatie();
    }

    // ════════════════════════════════════════════════════════════════════════
    //  UTILITARE PRIVATE
    // ════════════════════════════════════════════════════════════════════════

    private static void Titlu(string text)
    {
        string linie = new string('═', Math.Max(text.Length + 4, 60));
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"  ╔{linie}╗");
        Console.WriteLine($"  ║  {text.PadRight(linie.Length - 2)}║");
        Console.WriteLine($"  ╚{linie}╝");
        Console.ResetColor();
    }

    private static void Separator() =>
        Console.WriteLine(new string('─', 60));
}
