using UnityEngine;
using System;
using System.Collections.Generic;

public class AbilityManager : MonoBehaviour
{
    public List<Ability> abilities = new List<Ability>();

    public void InitializeAbilities(Type[] abilityTypes)
    {
        foreach (var t in abilityTypes)
        {
            Ability ability = (Ability)Activator.CreateInstance(t);
            abilities.Add(ability);
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}