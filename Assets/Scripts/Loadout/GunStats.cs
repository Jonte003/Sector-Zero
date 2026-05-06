using Unity.Mathematics;

public class GunStats
{
    private GunSettings Gun { get; }

    public GunStats(GunSettings gun)
    {
        Gun = gun;
    }

    public int CalculateDamage()
    {
        return (int)math.clamp(math.round(
            GetDamage(0.75f) + 
            GetPierce(0.25f)
            ), 1, 10);
    }

    public int CalculateFireRate()
    {
        return (int)math.clamp(math.round(
            GetFireRate(1f)
            ), 1, 10);
    }

    public int CalculateReloadSpeed()
    {
        return (int)math.clamp(math.round(
            GetReloadSpeed(1f)
            ), 1, 10);
    }

    public int CalculateMagazineSize()
    {
        return (int)math.clamp(math.round(
            GetMagazineSize(1f)
            ), 1, 10);
    }

    public int CalculateAccuracy()
    {
        return (int)math.clamp(math.round
            (GetSpread(0.7f) + 
            GetRecoil(0.3f)
            ), 1, 10);
    }

    public int CalculateRange()
    {
        return (int)math.clamp(math.round(
            GetRange(0.8f) + 
            GetSpread(0.2f)
            ), 1, 10);
    }

    public int CalculateWeight()
    {
        return (int)math.clamp(math.round(
            GetMoveSpeed(1f)
            ), 1, 10);
    }

    private float GetDamage(float relevance)
    {
        float max = 40;

        return Gun.Damage * Gun.BulletCount / max * relevance * 10;
    }

    private float GetPierce(float relevance)
    {
        float max = 3;

        return 100 / Gun.PierceFalloff / max * relevance * 10;
    }

    private float GetFireRate(float relevance)
    {
        float max = 10;

        return Gun.FireRate / max * relevance * 10;
    }

    private float GetReloadSpeed(float relevance)
    {
        float max = 5;

        return (max - Gun.ReloadSpeed) / max * relevance * 10;
    }

    private float GetMagazineSize(float relevance)
    {
        float max = 100;

        return Gun.MaxAmmo / max * relevance * 10;
    }

    private float GetSpread(float relevance)
    {
        float max = 10;

        return (1 - (Gun.MaxSpread.x + Gun.MaxSpread.y) / max) * relevance * 10;
    }

    private float GetRecoil(float relevance)
    {
        float max = 10;

        return math.saturate(1 - ((Gun.RecoilMagnitude.x + Gun.RecoilMagnitude.y) / 2) * ((Gun.RecoilMax + Gun.RecoilMin) / 2) / max) * relevance * 10;
    }

    private float GetRange(float relevance)
    {
        float max = 200f;

        float latestRange = 0;

        float totalDamage = 0;

        for (int i = 0; i < Gun.DamageFalloffRange.Length; i++)
        {
            totalDamage += Gun.DamageFalloffPercentage[i] * (Gun.DamageFalloffRange[i] - latestRange);
            latestRange = Gun.DamageFalloffRange[i];
        }

        float averageDmgMult = totalDamage / Gun.Range;

        return Gun.Range * averageDmgMult / max * relevance * 10;
    }

    private float GetMoveSpeed(float relevance)
    {
        float max = 1.6f;

        return (1 - Gun.MoveSpeed / max) * relevance * 10;
    }
}