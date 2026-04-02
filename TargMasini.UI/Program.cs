using TargMasini.Entitati;
using TargMasini.Entitati.Enums;
using TargMasini.Logica.Memorie;
using TargMasini.Logica.StocareDate;
using TargMasini.UI.Rapoarte;

// ════════════════════════════════════════════════════════════════════════════
//  INIȚIALIZARE
// ════════════════════════════════════════════════════════════════════════════

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.Title = "Târg de Mașini — Sistem de Gestiune";

// Strat memorie
var gestTranzactii = new GestiuneTranzactii();
var gestAuto       = new GestiuneAuto();

// Strat stocare date (fișier text) — tema 7
var fisierTranzactii = new FisierTranzactii("tranzactii.txt");
var fisierAuto       = new FisierAuto("masini.txt");

// Încarcă din fișiere dacă există
foreach (var t in fisierTranzactii.IncarcaToate()) gestTranzactii.AdaugaObiect(t);
foreach (var a in fisierAuto.IncarcaToate())       gestAuto.AdaugaObiect(a);

// Dacă nu există date în fișiere, populăm cu date demo
if (!gestTranzactii.GetToate().Any())
    IncarcaDateDemo(gestTranzactii, gestAuto);

var raport = new RaportTarg(gestTranzactii, gestAuto);

// ════════════════════════════════════════════════════════════════════════════
//  MENIU PRINCIPAL
// ════════════════════════════════════════════════════════════════════════════

bool continua = true;
while (continua)
{
    AfiseazaMeniu();
    string? optiune = Console.ReadLine()?.Trim();

    switch (optiune)
    {
        // ── Rapoarte ──────────────────────────────────────────────────────
        case "1": raport.AfiseazaRaportComplet();       break;
        case "2": raport.AfiseazaToateTranzactiile();   break;

        case "3":
            Console.Write("\n  Introduceți firma: ");
            string? f = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(f)) raport.AfiseazaTranzactiiFirma(f);
            break;

        case "4":
            Console.Write("\n  Preț minim : "); decimal.TryParse(Console.ReadLine(), out decimal pMin);
            Console.Write("  Preț maxim : "); decimal.TryParse(Console.ReadLine(), out decimal pMax);
            raport.AfiseazaTranzactiiInterval(pMin, pMax);
            break;

        case "5":
            Console.Write("\n  Culoare (0=Rosu,1=Alb,2=Negru,3=Gri,4=Albastru,...): ");
            if (int.TryParse(Console.ReadLine(), out int culoareIdx) &&
                Enum.IsDefined(typeof(CuloareAuto), culoareIdx))
            {
                var culoare = (CuloareAuto)culoareIdx;
                var lista = gestTranzactii.DupaCuloare(culoare).ToList();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n  Tranzacții {culoare}: {lista.Count} rezultate\n");
                Console.ResetColor();
                foreach (var t in lista) Console.WriteLine(t);
            }
            break;

        case "6":
            AfiseazaOptiuniDisponibile();
            Console.Write("  Bifați opțiunea (valoare numerică): ");
            if (int.TryParse(Console.ReadLine(), out int optVal))
            {
                var optCautata = (OptiuniAuto)optVal;
                var lista = gestTranzactii.CuOptiune(optCautata).ToList();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n  Mașini cu opțiunea '{optCautata}': {lista.Count} rezultate\n");
                Console.ResetColor();
                foreach (var t in lista) Console.WriteLine(t);
            }
            break;

        // ── Tranzacții CRUD ───────────────────────────────────────────────
        case "7": AfiseazaAdaugaTranzactie(gestTranzactii, fisierTranzactii); break;

        case "8":
            Console.Write("\n  ID tranzacție de șters: ");
            if (int.TryParse(Console.ReadLine(), out int idT))
            {
                bool ok = gestTranzactii.Sterge(idT);
                if (ok) fisierTranzactii.StergeDinFisier(idT);
                Console.WriteLine(ok ? $"  ✓ Tranzacția #{idT} ștearsă." : $"  ✗ ID #{idT} negăsit.");
            }
            break;

        case "9":
            Console.Write("\n  ID tranzacție: ");
            if (int.TryParse(Console.ReadLine(), out int idMod))
            {
                Console.Write("  Preț nou: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal pretNou))
                {
                    fisierTranzactii.ModificaPret(idMod, pretNou);
                    var tr = gestTranzactii.GasesteDupaId(idMod);
                    if (tr is not null) { tr.Pret = pretNou; Console.WriteLine("  ✓ Preț actualizat."); }
                }
            }
            break;

        // ── Auto (a 2-a entitate) ─────────────────────────────────────────
        case "A": raport.AfiseazaMasiniDisponibile();             break;
        case "B": AfiseazaAdaugaAuto(gestAuto, fisierAuto);       break;
        case "C":
            Console.Write("\n  ID mașină de marcat vândută: ");
            if (int.TryParse(Console.ReadLine(), out int idAuto))
            {
                bool ok = gestAuto.MarcheazaVanduta(idAuto);
                if (ok) fisierAuto.MarcheazaVanduta(idAuto);
                Console.WriteLine(ok ? "  ✓ Marcată ca vândută." : "  ✗ ID negăsit.");
            }
            break;

        // ── StocareDate ───────────────────────────────────────────────────
        case "S":
            fisierTranzactii.SalveazaToate(gestTranzactii.GetToate());
            fisierAuto.SalveazaToate(gestAuto.GetToate());
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ✓ Date salvate în fișiere (tranzactii.txt, masini.txt)");
            Console.ResetColor();
            break;

        case "0":
            // Salvează automat la ieșire
            fisierTranzactii.SalveazaToate(gestTranzactii.GetToate());
            fisierAuto.SalveazaToate(gestAuto.GetToate());
            continua = false;
            Console.WriteLine("\n  Date salvate. La revedere!");
            break;

        default:
            Console.WriteLine("  Opțiune invalidă.");
            break;
    }

    if (continua && optiune != "0")
    {
        Console.WriteLine("\n  [Enter] pentru meniu...");
        Console.ReadLine();
    }
}

// ════════════════════════════════════════════════════════════════════════════
//  FUNCȚII LOCALE
// ════════════════════════════════════════════════════════════════════════════

static void AfiseazaMeniu()
{
    try { Console.Clear(); } catch { }
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("""
        ╔══════════════════════════════════════════════════════╗
        ║       TÂRG DE MAȘINI — Sistem de Gestiune          ║
        ╠══════════════════════════════════════════════════════╣
        ║  ── RAPOARTE ──────────────────────────────────  ║
        ║  1. Raport complet                               ║
        ║  2. Toate tranzacțiile                           ║
        ║  3. Filtrare după firmă                          ║
        ║  4. Filtrare după interval de preț               ║
        ║  5. Filtrare după culoare (enum)                 ║
        ║  6. Filtrare după opțiune (flags enum)           ║
        ║  ── TRANZACȚII (CRUD) ─────────────────────────  ║
        ║  7. Adaugă tranzacție nouă                       ║
        ║  8. Șterge tranzacție                            ║
        ║  9. Modifică preț tranzacție                     ║
        ║  ── MAȘINI — a 2-a entitate ───────────────────  ║
        ║  A. Afișează mașini disponibile                  ║
        ║  B. Adaugă mașină în catalog                     ║
        ║  C. Marchează mașină ca vândută                  ║
        ║  ── STOCARE DATE ──────────────────────────────  ║
        ║  S. Salvează manual în fișiere text              ║
        ║──────────────────────────────────────────────────║
        ║  0. Ieșire (salvare automată)                    ║
        ╚══════════════════════════════════════════════════════╝
        """);
    Console.ResetColor();
    Console.Write("  Opțiunea: ");
}

static void AfiseazaOptiuniDisponibile()
{
    Console.WriteLine("\n  Opțiuni disponibile:");
    foreach (var o in Enum.GetValues<OptiuniAuto>().Where(o => o != OptiuniAuto.Niciuna))
        Console.WriteLine($"    {(int)o,6} = {o}");
}


// ─── Citire date tranzacție de la tastatură (cerință temă: citire de la tastatură) ───

static OptiuniAuto CitesteOptiuni()
{
    AfiseazaOptiuniDisponibile();
    Console.Write("  Introduceți valorile dorite separate prin + (ex: 8+16+2): ");
    string? input = Console.ReadLine();
    OptiuniAuto rezultat = OptiuniAuto.Niciuna;
    if (!string.IsNullOrWhiteSpace(input))
    {
        foreach (var part in input.Split('+', StringSplitOptions.RemoveEmptyEntries))
        {
            if (int.TryParse(part.Trim(), out int val))
                rezultat |= (OptiuniAuto)val;
        }
    }
    return rezultat;
}

static CuloareAuto CiteşteCuloare()
{
    Console.WriteLine("\n  Culori disponibile:");
    foreach (var c in Enum.GetValues<CuloareAuto>())
        Console.WriteLine($"    {(int)c} = {c}");
    Console.Write("  Alege culoarea (număr): ");
    int.TryParse(Console.ReadLine(), out int idx);
    return Enum.IsDefined(typeof(CuloareAuto), idx) ? (CuloareAuto)idx : CuloareAuto.Negru;
}

static void AfiseazaAdaugaTranzactie(GestiuneTranzactii gestiune, FisierTranzactii fisier)
{
    try { Console.Clear(); } catch { }
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("  ╔══════════════════════════════╗");
    Console.WriteLine("  ║  ADAUGĂ TRANZACȚIE NOUĂ     ║");
    Console.WriteLine("  ╚══════════════════════════════╝\n");
    Console.ResetColor();

    Console.Write("  Vânzător      : "); string vanz = Console.ReadLine() ?? "";
    Console.Write("  Cumpărător    : "); string cump = Console.ReadLine() ?? "";
    Console.Write("  Firmă         : "); string firma = Console.ReadLine() ?? "";
    Console.Write("  Model         : "); string model = Console.ReadLine() ?? "";
    Console.Write("  An fabricație : "); int.TryParse(Console.ReadLine(), out int an);

    var culoare = CiteşteCuloare();
    var optiuni = CitesteOptiuni();

    Console.Write("  Preț (RON)    : "); decimal.TryParse(Console.ReadLine(), out decimal pret);
    Console.Write("  Data (dd/MM/yyyy, Enter=azi): ");
    string dataStr = Console.ReadLine() ?? "";
    DateTime data = DateTime.TryParseExact(dataStr, "dd/MM/yyyy",
        System.Globalization.CultureInfo.InvariantCulture,
        System.Globalization.DateTimeStyles.None, out DateTime d) ? d : DateTime.Today;

    var t = gestiune.Adauga(vanz, cump, firma, model, an, culoare, optiuni, data, pret);
    fisier.AdaugaInFisier(t);

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"\n  ✓ Tranzacție adăugată (ID #{t.Id:D4})");
    Console.ResetColor();
    Console.WriteLine(t);
}

static void AfiseazaAdaugaAuto(GestiuneAuto gestiune, FisierAuto fisier)
{
    try { Console.Clear(); } catch { }
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine("  ╔══════════════════════════════╗");
    Console.WriteLine("  ║  ADAUGĂ MAȘINĂ ÎN CATALOG   ║");
    Console.WriteLine("  ╚══════════════════════════════╝\n");
    Console.ResetColor();

    Console.Write("  Firmă         : "); string firma = Console.ReadLine() ?? "";
    Console.Write("  Model         : "); string model = Console.ReadLine() ?? "";
    Console.Write("  An fabricație : "); int.TryParse(Console.ReadLine(), out int an);
    Console.Write("  Preț (RON)    : "); decimal.TryParse(Console.ReadLine(), out decimal pret);
    Console.Write("  Descriere     : "); string desc = Console.ReadLine() ?? "";

    var culoare = CiteşteCuloare();
    var optiuni = CitesteOptiuni();

    var a = gestiune.Adauga(firma, model, an, culoare, optiuni, pret, desc);
    fisier.AdaugaInFisier(a);

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"\n  ✓ Mașină adăugată (ID #{a.Id:D3})");
    Console.ResetColor();
    Console.WriteLine(a);
    Console.WriteLine($"  Opțiuni: {a.OptiuniText()}");
}

// ─── Date de demonstrație ─────────────────────────────────────────────────

static void IncarcaDateDemo(GestiuneTranzactii gestTranzactii, GestiuneAuto gestAuto)
{
    gestTranzactii.Adauga("Ion Popescu",       "Maria Ionescu",   "BMW",          "Seria 5",  2021, CuloareAuto.Negru,     OptiuniAuto.Trapa | OptiuniAuto.ScauneIncalzite | OptiuniAuto.Navigatie | OptiuniAuto.Xenon,                          new DateTime(2024, 3, 15),  45_000m);
    gestTranzactii.Adauga("Gheorghe Marin",    "Alexandru Dobre", "Audi",         "A6",       2022, CuloareAuto.Alb,       OptiuniAuto.TractiuteIntegrala | OptiuniAuto.FaruriLED | OptiuniAuto.ScauneIncalzite | OptiuniAuto.PilotAutomat,       new DateTime(2024, 4,  2),  52_500m);
    gestTranzactii.Adauga("Ion Popescu",       "Elena Constantin","Mercedes-Benz","Clasa C",  2020, CuloareAuto.Gri,       OptiuniAuto.PachetSport | OptiuniAuto.TrapaPanoramica | OptiuniAuto.Navigatie | OptiuniAuto.Camera360,               new DateTime(2024, 4, 10),  38_900m);
    gestTranzactii.Adauga("Vasile Dumitrescu", "Radu Petrescu",   "Volkswagen",   "Passat",   2019, CuloareAuto.Albastru,  OptiuniAuto.CutieAutomata | OptiuniAuto.Navigatie | OptiuniAuto.Xenon,                                              new DateTime(2024, 5, 20),  22_300m);
    gestTranzactii.Adauga("Gheorghe Marin",    "Ioana Stan",      "BMW",          "X5",       2023, CuloareAuto.Alb,       OptiuniAuto.TractiuteIntegrala | OptiuniAuto.TrapaPanoramica | OptiuniAuto.ScauneIncalzite | OptiuniAuto.PilotAutomat, new DateTime(2024, 6,  5),  78_000m);
    gestTranzactii.Adauga("Mihai Stoica",      "Cristina Varga",  "Dacia",        "Duster",   2022, CuloareAuto.Verde,     OptiuniAuto.TractiuteIntegrala | OptiuniAuto.Navigatie | OptiuniAuto.CameraMarSarier,                               new DateTime(2024, 6, 18),  18_500m);
    gestTranzactii.Adauga("Vasile Dumitrescu", "Florin Neacsu",   "Audi",         "Q7",       2021, CuloareAuto.Negru,     OptiuniAuto.TractiuteIntegrala | OptiuniAuto.FaruriLED | OptiuniAuto.Navigatie | OptiuniAuto.PilotAutomat | OptiuniAuto.Camera360, new DateTime(2024, 7, 3),  67_000m);
    gestTranzactii.Adauga("Mihai Stoica",      "Daniela Lupu",    "Mercedes-Benz","GLE",      2023, CuloareAuto.Rosu,      OptiuniAuto.PachetSport | OptiuniAuto.TrapaPanoramica | OptiuniAuto.Camera360 | OptiuniAuto.ScauneIncalzite | OptiuniAuto.Navigatie, new DateTime(2024, 7, 22), 89_000m);
    gestTranzactii.Adauga("Ion Popescu",       "Bogdan Serban",   "Renault",      "Clio",     2020, CuloareAuto.Portocaliu, OptiuniAuto.Navigatie | OptiuniAuto.PilotAutomat,                                                                 new DateTime(2024, 8,  7),  14_200m);
    gestTranzactii.Adauga("Gheorghe Marin",    "Nicoleta Oprea",  "Volkswagen",   "Tiguan",   2022, CuloareAuto.Gri,       OptiuniAuto.TractiuteIntegrala | OptiuniAuto.CutieAutomata | OptiuniAuto.Trapa | OptiuniAuto.Navigatie | OptiuniAuto.Xenon,      new DateTime(2024, 9, 14),  35_800m);

    gestAuto.Adauga("BMW",          "Seria 3",  2023, CuloareAuto.Argintiu, OptiuniAuto.Navigatie | OptiuniAuto.ScauneIncalzite | OptiuniAuto.Xenon,              35_000m, "Stare excelentă, garanție activă");
    gestAuto.Adauga("Audi",         "A4",       2022, CuloareAuto.Negru,    OptiuniAuto.TractiuteIntegrala | OptiuniAuto.Navigatie | OptiuniAuto.PilotAutomat,   42_000m, "Un singur proprietar");
    gestAuto.Adauga("Toyota",       "Corolla",  2021, CuloareAuto.Alb,      OptiuniAuto.AerConditionat | OptiuniAuto.Navigatie | OptiuniAuto.CameraMarSarier,    19_500m, "Hibrid, consum redus");
    gestAuto.Adauga("Ford",         "Focus",    2020, CuloareAuto.Albastru,  OptiuniAuto.AerConditionat | OptiuniAuto.Navigatie,                                  14_800m);
    gestAuto.Adauga("Mercedes-Benz","C 200",    2023, CuloareAuto.Gri,       OptiuniAuto.PachetSport | OptiuniAuto.ScauneIncalzite | OptiuniAuto.Navigatie,       58_000m, "Pachet AMG, jante 18\"");
}
