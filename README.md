# 🚗 Târg de Mașini — Sistem de Gestiune C#

Proiect C# (.NET 8) cu arhitectură **Entitate → Relație → Raport**, similar unui sistem de bibliotecă.

## Cum se compilează și rulează

```bash
cd TargMasini
dotnet run
```

## Arhitectura proiectului

```
TargMasini/
├── Entitati/
│   ├── TranzactieAuto.cs   ← Entitatea principală
│   ├── Auto.cs             ← Entitatea vehicul
│   └── Persoana.cs         ← Entitatea persoană
├── Relatii/
│   └── GestiuneTranzactii.cs  ← Logica + interogări LINQ
├── Rapoarte/
│   └── RaportTarg.cs          ← Afișare rapoarte formatate
├── Program.cs              ← Meniu interactiv + date demo
└── TargMasini.csproj
```

## Clasa TranzactieAuto

| Proprietate      | Tip            | Descriere                         |
|------------------|----------------|-----------------------------------|
| `NumeVanzator`   | `string`       | Numele vânzătorului               |
| `NumeCumparator` | `string`       | Numele cumpărătorului             |
| `Firma`          | `string`       | Marca mașinii (BMW, Audi, etc.)   |
| `Model`          | `string`       | Modelul vehiculului               |
| `AnFabricatie`   | `int`          | Anul de fabricație                |
| `Culoare`        | `string`       | Culoarea vehiculului              |
| `Optiuni`        | `List<string>` | Lista de dotări/opțiuni           |
| `DataTranzactie` | `DateTime`     | Data realizării tranzacției       |
| `Pret`           | `decimal`      | Prețul de vânzare (RON)           |

## Interogări LINQ implementate

### Filtrări
- `DupaFirma(firma)` — filtrare după marcă
- `DupaVanzator(numePartial)` — filtrare după vânzător
- `DupaCumparator(numePartial)` — filtrare după cumpărător
- `DupaInterval(pretMin, pretMax)` — interval de prețuri
- `DupaAnFabricatie(anMin, anMax)` — interval de ani
- `DupaPerioada(de, panaLa)` — interval de date
- `CuOptiune(optiune)` — mașini cu o anumită dotare

### Sortări
- `SortateDupaPrețAsc()` / `SortateDupaPrețDesc()`
- `SortateDupaDataDesc()`

### Statistici (agregate)
- `ValoareTotala()` — suma tuturor tranzacțiilor
- `PretMediu()` — media prețurilor
- `CelMaiScump()` / `CelMaiIeftin()` — vehiculele extreme
- `GrupariPeFirma()` — GroupBy marcă + Count
- `TotalPeVanzator()` — GroupBy vânzător + Sum
- `OptiuniPopulare()` — SelectMany + GroupBy frecvență
- `DistributieAnFabricatie()` — GroupBy an + Average
