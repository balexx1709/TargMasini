namespace TargMasini.Logica.Memorie;

/// <summary>
/// Interfață generică pentru administrarea în memorie a oricărei entități.
/// Definește contractul de bază: adăugare, citire, ștergere.
/// GestiuneTranzactii și GestiuneAuto implementează această interfață.
/// </summary>
public interface IGestiuneEntitate<T>
{
    /// <summary>Adaugă un obiect existent în colecție.</summary>
    void AdaugaObiect(T entitate);

    /// <summary>Returnează toate entitățile din colecție (doar citire).</summary>
    IReadOnlyList<T> GetToate();

    /// <summary>Caută o entitate după ID. Returnează null dacă nu există.</summary>
    T? GasesteDupaId(int id);

    /// <summary>Șterge o entitate după ID. Returnează true dacă a reușit.</summary>
    bool Sterge(int id);
}
