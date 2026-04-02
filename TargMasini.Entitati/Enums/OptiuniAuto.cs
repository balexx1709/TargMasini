namespace TargMasini.Entitati.Enums;

/// <summary>
/// Enumerare cu atribut [Flags] pentru opțiunile unui automobil.
/// Permite combinarea mai multor opțiuni cu operatorul | (OR pe biți).
/// Cerință temă: câmp enum cu [Flags] pentru opțiuni multiple.
/// 
/// Exemplu utilizare:
///   OptiuniAuto opt = OptiuniAuto.Navigatie | OptiuniAuto.Trapa | OptiuniAuto.Xenon;
///   bool areNavigatie = opt.HasFlag(OptiuniAuto.Navigatie); // true
/// </summary>
[Flags]
public enum OptiuniAuto
{
    Niciuna            = 0,
    AerConditionat     = 1 << 0,   //    1
    Navigatie          = 1 << 1,   //    2
    CutieAutomata      = 1 << 2,   //    4
    Trapa              = 1 << 3,   //    8
    ScauneIncalzite    = 1 << 4,   //   16
    Xenon              = 1 << 5,   //   32
    PilotAutomat       = 1 << 6,   //   64
    Camera360          = 1 << 7,   //  128
    TractiuteIntegrala = 1 << 8,   //  256
    JanteAliaj         = 1 << 9,   //  512
    TrapaPanoramica    = 1 << 10,  // 1024
    FaruriLED          = 1 << 11,  // 2048
    PachetSport        = 1 << 12,  // 4096
    CameraMarSarier    = 1 << 13   // 8192
}
