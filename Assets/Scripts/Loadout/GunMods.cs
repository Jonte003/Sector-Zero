using System.Runtime.InteropServices;

public abstract class GunMod
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public virtual float WeaponDamage { get; protected set; } = 0f;
    public virtual float FireRate { get; protected set; } = 0f;
    public virtual float Spread { get; protected set; } = 0f;
    public virtual float Recoil { get; protected set; } = 0f;
    public virtual float MoveSpeed { get; protected set; } = 0f;
    public virtual float MagSize { get; protected set; } = 0f;
    public virtual float ReloadSpeed { get; protected set; } = 0f;
}

public class Assignable : GunMod
{
    public override string Name => "";
    public override string Description => "";

    public Assignable(float weaponDamage, float fireRate, float spread, float recoil, float moveSpeed, float magSize, float reloadSpeed)
    {
        WeaponDamage = weaponDamage;
        FireRate = fireRate;
        Spread = spread;
        Recoil = recoil;
        MoveSpeed = moveSpeed;
        MagSize = magSize;
        ReloadSpeed = reloadSpeed;
    }
}

public class Blank : GunMod
{
    public override string Name => "Blank";
    public override string Description => "No modifications";
}

public class LongBarrel : GunMod
{
    public override string Name => "Long Barrel";
    public override string Description => "Increases damage and decreases spread at the cost of move speed and fire rate";
    public override float WeaponDamage => 0.1f;
    public override float Spread => -0.1f;
    public override float MoveSpeed => -0.1f;
    public override float FireRate => -0.1f;
}

public class ShortBarrel : GunMod
{
    public override string Name => "Short Barrel";
    public override string Description => "Increases fire rate and move speed at the cost of damage and decreased spread";
    public override float WeaponDamage => -0.1f;
    public override float Spread => 0.1f;
    public override float MoveSpeed => 0.1f;
    public override float FireRate => 0.1f;
}

public class PortedBarrel : GunMod
{
    public override string Name => "Ported Barrel";
    public override string Description => "Increases fire rate and decreases recoil at the cost of damage and increased spread";
    public override float WeaponDamage => -0.1f;
    public override float Spread => 0.1f;
    public override float Recoil => -0.1f;
    public override float FireRate => 0.1f;
}

public class ExtendedMagazine : GunMod
{
    public override string Name => "Extended Magazine";
    public override string Description => "Increases magazine size at the cost of move speed, fire rate and reload speed";
    public override float MoveSpeed => -0.05f;
    public override float FireRate => -0.05f;
    public override float ReloadSpeed => -0.1f;
    public override float MagSize => 0.2f;
}

public class DrumMagazine : GunMod
{
    public override string Name => "Drum Magazine";
    public override string Description => "Largely increases magazine size at the cost of move speed, fire rate, increased spread and reload speed";
    public override float MoveSpeed => -0.1f;
    public override float FireRate => -0.05f;
    public override float Spread => 0.05f;
    public override float MagSize => 0.3f;
    public override float ReloadSpeed => -0.1f;
}

public class FastMagazine : GunMod
{
    public override string Name => "Fast Magazine";
    public override string Description => "Increases reload speed, move speed and decreases spread at the cost of magazine size and increased recoil";
    public override float MagSize => -0.15f;
    public override float ReloadSpeed => 0.1f;
    public override float MoveSpeed => 0.1f;
    public override float Spread => -0.05f;
    public override float Recoil => 0.1f;
}

public class VerticalGrip : GunMod
{
    public override string Name => "Vertical Grip";
    public override string Description => "Decreases recoil and spread at the cost of move speed and fire rate";
    public override float Recoil => -0.1f;
    public override float Spread => -0.1f;
    public override float MoveSpeed => -0.1f;
    public override float FireRate => -0.1f;
}

public class AngledGrip : GunMod
{
    public override string Name => "Angled Grip";
    public override string Description => "Decreases recoil and increases fire rate at the cost of move speed and increased spread";
    public override float Recoil => -0.1f;
    public override float MoveSpeed => -0.1f;
    public override float FireRate => 0.1f;
    public override float Spread => 0.1f;
}

public class ErgonomicGrip : GunMod
{
    public override string Name => "Ergonomic Grip";
    public override string Description => "Increases movespeed and decreases spead at the cost of increased recoil and decreased damage";
    public override float Recoil => 0.1f;
    public override float MoveSpeed => 0.1f;
    public override float WeaponDamage => -0.1f;
    public override float Spread => -0.1f;
}

public class HeavyStock : GunMod
{
    public override string Name => "Heavy Stock";
    public override string Description => "Decreases recoil and spread at the cost of move speed and fire rate";
    public override float Recoil => -0.1f;
    public override float Spread => -0.1f;
    public override float MoveSpeed => -0.1f;
    public override float FireRate => -0.1f;
}

public class LightStock : GunMod
{
    public override string Name => "Light Stock";
    public override string Description => "Increases move speed and fire rate at the cost of increased recoil and spread";
    public override float Recoil => 0.1f;
    public override float Spread => 0.1f;
    public override float MoveSpeed => 0.1f;
    public override float FireRate => 0.1f;
}

public class BalancedStock : GunMod
{
    public override string Name => "Balanced Stock";
    public override string Description => "Slightly decreases recoil and slightly increases move speed at the cost of slightly decreased damage and slightly increased spread";
    public override float Recoil => -0.05f;
    public override float MoveSpeed => 0.05f;
    public override float WeaponDamage => -0.05f;
    public override float Spread => 0.05f;
}

public class Suppressor : GunMod
{
    public override string Name => "Suppressor";
    public override string Description => "Decreases recoil and spread at the cost of damage and move speed";
    public override float WeaponDamage => -0.1f;
    public override float MoveSpeed => -0.1f;
    public override float Recoil => -0.1f;
    public override float Spread => -0.1f;
}

public class MuzzleBreak : GunMod
{
    public override string Name => "Muzzle Break";
    public override string Description => "Heavily decreases recoil and slightly decreases spread at the cost of move speed and fire rate";
    public override float Spread => -0.05f;
    public override float MoveSpeed => -0.1f;
    public override float Recoil => -0.15f;
    public override float FireRate => -0.1f;
}

public class Compensator : GunMod
{
    public override string Name => "Compensator";
    public override string Description => "Decreases recoil and increases fire rate at the cost of move speed and increased spread";
    public override float Recoil => -0.1f;
    public override float MoveSpeed => -0.1f;
    public override float FireRate => 0.1f;
    public override float Spread => 0.1f;
}