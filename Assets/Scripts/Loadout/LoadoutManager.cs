using UnityEngine;

public static class LoadoutManager
{
    public static Gun Gun { get; set; }
    public static GunMod[] GunMods { get; set; }
    public static Ability Ability1 { get; set; }
    public static Ability Ability2 { get; set; }
    public static Ability Ability3 { get; set; }
    public static Ability[] AbilitiesInBag { get; set; }
    public static GameObject GunPrefab { get; set; }
    public static GunSettings Settings { get; set; }
}