using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerLevels : MonoBehaviour
{
    public float Experience { get; private set; }
    public float NextLevelExperience { get; private set; }
    public int Level { get; private set; }
    public bool PendingLevelUp { get; private set; }

    public void Start()
    {
        Level = 1;
        Experience = 0;
        NextLevelExperience = ExperienceForLevelUp(Level + 1);
        PendingLevelUp = false;
    }

    public void AddExperience(float amount)
    {
        Experience += amount;
        CheckLevelUp();
    }
    public void ConfirmLevelUp()
    {
        if (PendingLevelUp)
        {
            Level++;
            Experience -= NextLevelExperience;
            NextLevelExperience = ExperienceForLevelUp(Level + 1);
            PendingLevelUp = false;
        }
    }

    private void CheckLevelUp()
    {
        if (Experience >= NextLevelExperience)
        {
            PendingLevelUp = true;
        }
    }

    private float ExperienceForLevelUp(int level)
    {
        return Mathf.Round(10 * Mathf.Pow(level - 1, 1.2f));
    }
}