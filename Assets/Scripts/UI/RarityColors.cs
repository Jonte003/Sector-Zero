using UnityEngine;

public class RarityColors : MonoBehaviour
{
    [SerializeField] Color commonColor;
    [SerializeField] Color uncommonColor;
    [SerializeField] Color rareColor;
    [SerializeField] Color epicColor;
    [SerializeField] Color legendaryColor;

    public Color GetColor(StatRarity rarity)
    {
        if (rarity == StatRarity.Common) return commonColor;
        if (rarity == StatRarity.Uncommon) return uncommonColor;
        if (rarity == StatRarity.Rare) return rareColor;
        if (rarity == StatRarity.Epic) return epicColor;
        if (rarity == StatRarity.Legendary) return legendaryColor;
        return Color.magenta;
    }
}
