using TargMasini.Entitati;
using TargMasini.Entitati.Enums;
using TargMasini.Logica.Memorie;

namespace TargMasini.UI.Rapoarte;

/// <summary>
/// Generează rapoarte formatate pe baza datelor din gestiune.
/// Strat UI / Raport — nu conține logică de business.
/// </summary>
public class RaportTarg
{
    private readonly GestiuneTranzactii _gestTranzactii;
    private readonly GestiuneAuto       _gestAuto;

    public RaportTarg(GestiuneTranzactii gestTranzactii, GestiuneAuto gestAuto)
    {
        _gestTranzactii = gestTranzactii;
        _gestAuto       = gestAuto;
    }

    // ════════════════════════════════════════════════════════════════════════
    //  RAPORT GENERAL
    // ════════════════════════════════════════════════════════════════════════

    public void AfiseazaRaportGeneral()
    {
        var toate  = _gestTranzactii.GetToate();
        var masini = _gestAuto.GetToate();

        Titlu("RAPORT GENERAL - TÂRG DE MAȘINI");
        Console.WriteLine($"  Data raportului  : {DateTime.Now:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"  Total tranzacții : {toate.Count}");
        Console.WriteLine($"  Total mașini     : {masini.Count} ({masini.Count(m => !m.EsteVanduta)} disponibile)");
        Console.WriteLine($"  Valoare totală   : {_gestTranzactii.ValoareTotala():C2}");
        Console.WriteLine($"  Preț mediu       : {_gestTranzactii.PretMediu():C2}");

        var maxT = _gestTranzactii.CelMaiScump();
        var minT = _gestTranzactii.CelMaiIeftin();
        if (maxT is not null)
            Console.WriteLine($"  Cel mai scump    : {maxT.Firma} {maxT.Model} - {maxT.Pret:C2}");
        if (minT is not null)
            Console.WriteLine($"  Cel mai ieftin   : {minT.Firma} {minT.Model} - {minT.Pret:C2}");
        Separator();
    }

    // ════════════════════════════════════════════════════════════════════════
    //  RAPORT TRANZACȚII
    // ════════════════════════════════════════════════════════════════════════

    public void AfiseazaToateTranzactiile()
    {
        Titlu("TOATE TRANZACȚIILE (ordonate după dată)");
        foreach (var t in _gestTranzactii.SortateDupaDataDesc())
            Console.WriteLine(t);
        Separator();
    }

    public void AfiseazaTranzactiiFirma(string firma)
    {
        var lista = _gestTranzactii.DupaFirma(firma).ToList();
        Titlu($"TRANZACȚII FIRMA: {firma.ToUpper()} — {lista.Count} rezultate");
        foreach (var t in lista.OrderByDescending(t => t.DataTranzactie))
            Console.WriteLine(t);
        Separator();
    }

    public void AfiseazaTranzactiiInterval(decimal pMin, decimal pMax)
    {
        var lista = _gestTranzactii.DupaInterval(pMin, pMax).ToList();
        Titlu($"TRANZACȚII {pMin:C0} – {pMax:C0} — {lista.Count} rezultate");
        foreach (var t in lista.OrderBy(t => t.Pret))
            Console.WriteLine(t);
        Separator();
    }

    // ════════════════════════════════════════════════════════════════════════
    //  RAPORT AUTO (a doua entitate)
    // ════════════════════════════════════════════════════════════════════════

    public void AfiseazaMasiniDisponibile()
    {
        Titlu("MAȘINI DISPONIBILE ÎN TÂRG");
        Console.WriteLine($"  {"ID",-5} {"Firmă Model (An)",-30} {"Culoare",-15} {"Preț",14}");
        Console.WriteLine(new string('─', 68));

        foreach (var a in _gestAuto.Disponibile().OrderBy(a => a.Pret))
        {
            Console.WriteLine($"  {a.Id,-5} {$"{a.Firma} {a.Model} ({a.AnFabricatie})",-30} {a.Culoare,-15} {a.Pret,14:C2}");
            Console.WriteLine($"  {"",-5} Opțiuni: {a.OptiuniText()}");
        }
        Separator();
    }

    // ════════════════════════════════════════════════════════════════════════
    //  STATISTICI
    // ════════════════════════════════════════════════════════════════════════

    public void AfiseazaStatisticiPeFirma()
    {
        Titlu("STATISTICI PE FIRMĂ");
        Console.WriteLine($"  {"Firmă",-20} {"Tranzacții",13} {"Total",18} {"Medie",16}");
        Console.WriteLine(new string('─', 70));

        foreach (var (firma, numar, total) in _gestTranzactii.GrupariPeFirma())
        {
            decimal medie = numar > 0 ? total / numar : 0;
            Console.WriteLine($"  {firma,-20} {numar,13} {total,18:C2} {medie,16:C2}");
        }
        Separator();
    }

    public void AfiseazaTopVanzatori()
    {
        Titlu("TOP VÂNZĂTORI");
        Console.WriteLine($"  {"#",-4} {"Vânzător",-25} {"Tranzacții",12} {"Total",16}");
        Console.WriteLine(new string('─', 60));

        var top = _gestTranzactii.TotalPeVanzator().ToList();
        for (int i = 0; i < top.Count; i++)
            Console.WriteLine($"  {i + 1,-4} {top[i].Vanzator,-25} {top[i].Numar,12} {top[i].Total,16:C2}");
        Separator();
    }

    public void AfiseazaOptiuniPopulare()
    {
        Titlu("OPȚIUNI CELE MAI SOLICITATE");
        Console.WriteLine($"  {"Opțiune",-25} {"Frecvență",10}  Bar");
        Console.WriteLine(new string('─', 55));

        var lista = _gestTranzactii.OptiuniPopulare().ToList();
        int maxF = lista.Count > 0 ? lista.Max(x => x.Frecventa) : 1;
        foreach (var (optiune, frecventa) in lista)
        {
            int barLen = (int)Math.Round((double)frecventa / maxF * 20);
            Console.WriteLine($"  {optiune,-25} {frecventa,10}  {new string('█', barLen)}");
        }
        Separator();
    }

    public void AfiseazaDistributieCulori()
    {
        Titlu("DISTRIBUȚIE CULORI — TRANZACȚII");
        Console.WriteLine($"  {"Culoare",-20} {"Nr.",8}  Bar");
        Console.WriteLine(new string('─', 50));

        var dist = _gestTranzactii.DistributieCulori().ToList();
        int maxN = dist.Count > 0 ? dist.Max(x => x.Numar) : 1;
        foreach (var (culoare, numar) in dist)
        {
            int barLen = (int)Math.Round((double)numar / maxN * 20);
            Console.WriteLine($"  {culoare,-20} {numar,8}  {new string('█', barLen)}");
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
        AfiseazaDistributieCulori();
        AfiseazaMasiniDisponibile();
    }

    // ─── Utilitare ────────────────────────────────────────────────────────────

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
