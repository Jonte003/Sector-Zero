using UnityEngine;

public class Loadout : MonoBehaviour
{
    public Gun Gun { get; set; }
    //public GunMods[] GunMods { get; private set; }
    public Ability Ability1 { get;  set; }
    public Ability Ability2 { get;  set; }
    public Ability Ability3 { get;  set; }
    public Ability[] AbilitiesInBag { get; set; }

    public void UpdateCooldowns()
    {
        Ability1.CurrentCD = Mathf.Max(0, Ability1.CurrentCD - Time.deltaTime);
        Ability2.CurrentCD = Mathf.Max(0, Ability2.CurrentCD - Time.deltaTime);
        Ability3.CurrentCD = Mathf.Max(0, Ability3.CurrentCD - Time.deltaTime);
    }

    public Ability[] GetRandomAbilities(int amount)
    {
        Ability[] abilities = new Ability[amount];
        for (int i = 0; i < amount; i++)
        {
            abilities[i] = AbilitiesInBag[Random.Range(0, AbilitiesInBag.Length)];
        }
        return abilities;
    }
}