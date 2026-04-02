using TargMasini.Entitati;
using TargMasini.Relatii;
using TargMasini.Rapoarte;

// ════════════════════════════════════════════════════════════════════════════
//  INIȚIALIZARE DATE DE DEMONSTRAȚIE
// ════════════════════════════════════════════════════════════════════════════

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.Title = "Târg de Mașini - Sistem de Gestiune";

var gestiune = new GestiuneTranzactii();

// Populăm cu date de test reprezentative
gestiune.Adauga(
    numeVanzator:   "Ion Popescu",
    numeCumparator: "Maria Ionescu",
    firma:          "BMW",
    model:          "Seria 5",
    anFabricatie:   2021,
    culoare:        "Negru Safir",
    optiuni:        new List<string> { "Trapa", "Scaune încălzite", "Navigație", "Xenon" },
    dataTranzactie: new DateTime(2024, 3, 15),
    pret:           45_000m);

gestiune.Adauga(
    numeVanzator:   "Gheorghe Marin",
    numeCumparator: "Alexandru Dobre",
    firma:          "Audi",
    model:          "A6",
    anFabricatie:   2022,
    culoare:        "Alb Glacier",
    optiuni:        new List<string> { "Quattro", "Matrix LED", "Scaune încălzite", "Pilot automat" },
    dataTranzactie: new DateTime(2024, 4, 2),
    pret:           52_500m);

gestiune.Adauga(
    numeVanzator:   "Ion Popescu",
    numeCumparator: "Elena Constantin",
    firma:          "Mercedes-Benz",
    model:          "Clasa C",
    anFabricatie:   2020,
    culoare:        "Gri Selenitiu",
    optiuni:        new List<string> { "AMG Line", "Panoramic", "Navigație", "Camera 360°" },
    dataTranzactie: new DateTime(2024, 4, 10),
    pret:           38_900m);

gestiune.Adauga(
    numeVanzator:   "Vasile Dumitrescu",
    numeCumparator: "Radu Petrescu",
    firma:          "Volkswagen",
    model:          "Passat",
    anFabricatie:   2019,
    culoare:        "Albastru Acapulco",
    optiuni:        new List<string> { "DSG", "Navigație", "Xenon" },
    dataTranzactie: new DateTime(2024, 5, 20),
    pret:           22_300m);

gestiune.Adauga(
    numeVanzator:   "Gheorghe Marin",
    numeCumparator: "Ioana Stan",
    firma:          "BMW",
    model:          "X5",
    anFabricatie:   2023,
    culoare:        "Alb Alpine",
    optiuni:        new List<string> { "xDrive", "Trapa panoramica", "Scaune încălzite", "Xenon", "Pilot automat" },
    dataTranzactie: new DateTime(2024, 6, 5),
    pret:           78_000m);

gestiune.Adauga(
    numeVanzator:   "Mihai Stoica",
    numeCumparator: "Cristina Varga",
    firma:          "Dacia",
    model:          "Duster",
    anFabricatie:   2022,
    culoare:        "Verde Highland",
    optiuni:        new List<string> { "4x4", "Navigație", "Camera marșarier" },
    dataTranzactie: new DateTime(2024, 6, 18),
    pret:           18_500m);

gestiune.Adauga(
    numeVanzator:   "Vasile Dumitrescu",
    numeCumparator: "Florin Neacsu",
    firma:          "Audi",
    model:          "Q7",
    anFabricatie:   2021,
    culoare:        "Negru Mythos",
    optiuni:        new List<string> { "Quattro", "Matrix LED", "Navigație", "Pilot automat", "Camera 360°" },
    dataTranzactie: new DateTime(2024, 7, 3),
    pret:           67_000m);

gestiune.Adauga(
    numeVanzator:   "Mihai Stoica",
    numeCumparator: "Daniela Lupu",
    firma:          "Mercedes-Benz",
    model:          "GLE",
    anFabricatie:   2023,
    culoare:        "Rosu Designo",
    optiuni:        new List<string> { "AMG Line", "Panoramic", "Camera 360°", "Scaune încălzite", "Navigație" },
    dataTranzactie: new DateTime(2024, 7, 22),
    pret:           89_000m);

gestiune.Adauga(
    numeVanzator:   "Ion Popescu",
    numeCumparator: "Bogdan Serban",
    firma:          "Renault",
    model:          "Clio",
    anFabricatie:   2020,
    culoare:        "Portocaliu Valencia",
    optiuni:        new List<string> { "Navigație", "Pilot automat" },
    dataTranzactie: new DateTime(2024, 8, 7),
    pret:           14_200m);

gestiune.Adauga(
    numeVanzator:   "Gheorghe Marin",
    numeCumparator: "Nicoleta Oprea",
    firma:          "Volkswagen",
    model:          "Tiguan",
    anFabricatie:   2022,
    culoare:        "Gri Urano",
    optiuni:        new List<string> { "4Motion", "DSG", "Trapa", "Navigație", "Xenon" },
    dataTranzactie: new DateTime(2024, 9, 14),
    pret:           35_800m);

// ════════════════════════════════════════════════════════════════════════════
//  MENIU INTERACTIV
// ════════════════════════════════════════════════════════════════════════════

var raport = new RaportTarg(gestiune);
bool continua = true;

while (continua)
{
    AfiseazaMeniu();
    string? optiune = Console.ReadLine()?.Trim();

    switch (optiune)
    {
        case "1":
            raport.AfiseazaRaportComplet();
            break;

        case "2":
            raport.AfiseazaToateTranzactiile();
            break;

        case "3":
            Console.Write("\n  Introduceți firma: ");
            string? firma = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(firma))
                raport.AfiseazaTranzactiiFirma(firma);
            break;

        case "4":
            Console.Write("\n  Preț minim (RON): ");
            if (decimal.TryParse(Console.ReadLine(), out decimal pMin))
            {
                Console.Write("  Preț maxim (RON): ");
                if (decimal.TryParse(Console.ReadLine(), out decimal pMax))
                    raport.AfiseazaTranzactiiInterval(pMin, pMax);
            }
            break;

        case "5":
            Console.Write("\n  An minim: ");
            if (int.TryParse(Console.ReadLine(), out int anMin))
            {
                Console.Write("  An maxim: ");
                if (int.TryParse(Console.ReadLine(), out int anMax))
                {
                    var lista = gestiune.DupaAnFabricatie(anMin, anMax).ToList();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"\n  Tranzacții {anMin}–{anMax}: {lista.Count} rezultate\n");
                    Console.ResetColor();
                    foreach (var t in lista.OrderBy(t => t.AnFabricatie))
                        Console.WriteLine(t);
                }
            }
            break;

        case "6":
            Console.Write("\n  Opțiunea căutată: ");
            string? opt = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(opt))
            {
                var lista = gestiune.CuOptiune(opt).ToList();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n  Mașini cu opțiunea '{opt}': {lista.Count} rezultate\n");
                Console.ResetColor();
                foreach (var t in lista)
                    Console.WriteLine(t);
            }
            break;

        case "7":
            AfiseazaAdaugare(gestiune);
            break;

        case "8":
            Console.Write("\n  ID tranzacție de șters: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                bool ok = gestiune.Sterge(id);
                Console.WriteLine(ok
                    ? $"  ✓ Tranzacția #{id} a fost ștearsă."
                    : $"  ✗ Tranzacția #{id} nu a fost găsită.");
            }
            break;

        case "0":
            continua = false;
            Console.WriteLine("\n  La revedere!");
            break;

        default:
            Console.WriteLine("  Opțiune invalidă. Apăsați Enter pentru a continua...");
            Console.ReadLine();
            break;
    }

    if (continua && optiune != "0")
    {
        Console.WriteLine("\n  Apăsați Enter pentru a reveni la meniu...");
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
        ╔══════════════════════════════════════════════════╗
        ║         TÂRG DE MAȘINI - Sistem de Gestiune     ║
        ╠══════════════════════════════════════════════════╣
        ║  1. Raport complet                              ║
        ║  2. Toate tranzacțiile (ordonate după dată)     ║
        ║  3. Filtrare după firmă                         ║
        ║  4. Filtrare după interval de preț              ║
        ║  5. Filtrare după an de fabricație              ║
        ║  6. Căutare după opțiune                        ║
        ║  7. Adaugă tranzacție nouă                      ║
        ║  8. Șterge tranzacție după ID                   ║
        ║─────────────────────────────────────────────────║
        ║  0. Ieșire                                      ║
        ╚══════════════════════════════════════════════════╝
        """);
    Console.ResetColor();
    Console.Write("  Alegeți opțiunea: ");
}

static void AfiseazaAdaugare(GestiuneTranzactii gestiune)
{
    try { Console.Clear(); } catch { }
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("  ╔═══════════════════════════════╗");
    Console.WriteLine("  ║   ADAUGĂ TRANZACȚIE NOUĂ     ║");
    Console.WriteLine("  ╚═══════════════════════════════╝\n");
    Console.ResetColor();

    Console.Write("  Vânzător        : "); string vanzator    = Console.ReadLine() ?? "";
    Console.Write("  Cumpărător      : "); string cumparator  = Console.ReadLine() ?? "";
    Console.Write("  Firmă           : "); string firma       = Console.ReadLine() ?? "";
    Console.Write("  Model           : "); string model       = Console.ReadLine() ?? "";
    Console.Write("  An fabricație   : ");
    int.TryParse(Console.ReadLine(), out int an);
    Console.Write("  Culoare         : "); string culoare     = Console.ReadLine() ?? "";
    Console.Write("  Opțiuni (sep. cu virgulă): ");
    string optStr = Console.ReadLine() ?? "";
    var optiuni = optStr.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.Trim())
                        .ToList();
    Console.Write("  Preț (RON)      : ");
    decimal.TryParse(Console.ReadLine(), out decimal pret);
    Console.Write("  Data (dd/MM/yyyy, Enter=azi): ");
    string dataStr = Console.ReadLine() ?? "";
    DateTime data = DateTime.TryParseExact(dataStr, "dd/MM/yyyy",
        System.Globalization.CultureInfo.InvariantCulture,
        System.Globalization.DateTimeStyles.None, out DateTime d) ? d : DateTime.Today;

    var t = gestiune.Adauga(vanzator, cumparator, firma, model,
                            an, culoare, optiuni, data, pret);

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"\n  ✓ Tranzacție adăugată cu succes (ID #{t.Id:D4})");
    Console.ResetColor();
    Console.WriteLine(t);
}
