public abstract class GunMod
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract GunModCategory Category { get; }
    public virtual float WeaponDamage { get; protected set; } = 0f;
    public virtual float FireRate { get; protected set; } = 0f;
    public virtual float Spread { get; protected set; } = 0f;
    public virtual float Recoil { get; protected set; } = 0f;
    public virtual float MoveSpeed { get; protected set; } = 0f;
    public virtual float MagSize { get; protected set; } = 0f;
    public virtual float ReloadSpeed { get; protected set; } = 0f;
}

public enum GunModCategory
{
    Barrel,
    Magazine,
    Grip,
    Stock,
    Muzzle,
    None
}

public class Assignable : GunMod
{
    public override string Name => "";
    public override string Description => "";
    public override GunModCategory Category => GunModCategory.None;

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

public class BasicBarrel : GunMod
{
    public override string Name => "Basic Barrel";
    public override string Description => "";
    public override GunModCategory Category => GunModCategory.Barrel;
}
public class BasicMagazine : GunMod
{
    public override string Name => "Basic Magazine";
    public override string Description => "";
    public override GunModCategory Category => GunModCategory.Magazine;
}
public class BasicGrip : GunMod
{
    public override string Name => "Basic Grip";
    public override string Description => "";
    public override GunModCategory Category => GunModCategory.Grip;
}
public class BasicStock : GunMod
{
    public override string Name => "Basic Stock";
    public override string Description => "";
    public override GunModCategory Category => GunModCategory.Stock;
}
public class BasicMuzzle : GunMod
{
    public override string Name => "Basic Muzzle";
    public override string Description => "";
    public override GunModCategory Category => GunModCategory.Muzzle;
}

public class LongBarrel : GunMod
{
    public override string Name => "Long Barrel";
    public override string Description => "+Accuracy, +Weight, -Fire Rate";
    public override GunModCategory Category => GunModCategory.Barrel;

    public override float Spread => -0.15f;
    public override float Recoil => -0.05f;
    public override float MoveSpeed => -0.10f;
    public override float FireRate => -0.10f;
}

public class ShortBarrel : GunMod
{
    public override string Name => "Short Barrel";
    public override string Description => "+Fire Rate, -Weight, -Damage, -Accuracy";
    public override GunModCategory Category => GunModCategory.Barrel;

    public override float WeaponDamage => -0.05f;
    public override float Spread => 0.15f;
    public override float Recoil => 0.05f;
    public override float MoveSpeed => 0.10f;
    public override float FireRate => 0.10f;
}

public class PortedBarrel : GunMod
{
    public override string Name => "Ported Barrel";
    public override string Description => "+Accuracy, +Fire Rate, +Weight, +Damage";
    public override GunModCategory Category => GunModCategory.Barrel;

    public override float Recoil => -0.15f;
    public override float MoveSpeed => -0.15f;
    public override float FireRate => 0.05f;
    public override float WeaponDamage => -0.05f;
}

public class ExtendedMagazine : GunMod
{
    public override string Name => "Extended Magazine";
    public override string Description => "+Magazine Size, +Weight, -Fire Rate, -Reload Speed";
    public override GunModCategory Category => GunModCategory.Magazine;

    public override float MoveSpeed => -0.05f;
    public override float FireRate => -0.05f;
    public override float ReloadSpeed => 0.10f;
    public override float MagSize => 0.20f;
}

public class DrumMagazine : GunMod
{
    public override string Name => "Drum Magazine";
    public override string Description => "++Magazine Size, +Weight, -Fire Rate, -Reload Speed, -Accuracy";
    public override GunModCategory Category => GunModCategory.Magazine;

    public override float MoveSpeed => -0.10f;
    public override float FireRate => -0.05f;
    public override float ReloadSpeed => 0.15f;
    public override float Spread => 0.05f;
    public override float Recoil => 0.05f;
    public override float MagSize => 0.40f;
}

public class FastMagazine : GunMod
{
    public override string Name => "Fast Magazine";
    public override string Description => "+Reload Speed, -Magazine Size, -Weight, -Accuracy";
    public override GunModCategory Category => GunModCategory.Magazine;

    public override float MagSize => -0.20f;
    public override float ReloadSpeed => -0.15f;
    public override float MoveSpeed => 0.10f;
    public override float Recoil => 0.05f;
}

public class VerticalGrip : GunMod
{
    public override string Name => "Vertical Grip";
    public override string Description => "+Accuracy, +Weight, -Fire Rate";
    public override GunModCategory Category => GunModCategory.Grip;

    public override float Recoil => -0.15f;
    public override float Spread => -0.10f;
    public override float MoveSpeed => -0.10f;
    public override float FireRate => -0.15f;
}

public class AngledGrip : GunMod
{
    public override string Name => "Angled Grip";
    public override string Description => "+Fire Rate, +Weight, -Damage, +/- Accuracy";
    public override GunModCategory Category => GunModCategory.Grip;

    public override float Recoil => -0.10f;
    public override float Spread => 0.05f;
    public override float MoveSpeed => -0.05f;
    public override float FireRate => 0.10f;
    public override float WeaponDamage => -0.10f;
}

public class ErgonomicGrip : GunMod
{
    public override string Name => "Ergonomic Grip";
    public override string Description => "+Move Speed, +Fire Rate, -Weight, -Damage, -Accuracy";
    public override GunModCategory Category => GunModCategory.Grip;

    public override float MoveSpeed => 0.10f;
    public override float Recoil => 0.10f;
    public override float WeaponDamage => -0.05f;
    public override float FireRate => 0.05f;
}

public class HeavyStock : GunMod
{
    public override string Name => "Heavy Stock";
    public override string Description => "+Accuracy, +Weight, -Fire Rate";
    public override GunModCategory Category => GunModCategory.Stock;

    public override float Recoil => -0.15f;
    public override float Spread => -0.10f;
    public override float MoveSpeed => -0.10f;
    public override float FireRate => -0.15f;
}

public class LightStock : GunMod
{
    public override string Name => "Light Stock";
    public override string Description => "+Move Speed, +Fire Rate, -Weight, -Accuracy";
    public override GunModCategory Category => GunModCategory.Stock;

    public override float Recoil => 0.15f;
    public override float Spread => 0.10f;
    public override float MoveSpeed => 0.10f;
    public override float FireRate => 0.15f;
}

public class BalancedStock : GunMod
{
    public override string Name => "Balanced Stock";
    public override string Description => "+Accuracy, +Move Speed, -Fire Rate, -Damage";
    public override GunModCategory Category => GunModCategory.Stock;

    public override float Recoil => -0.05f;
    public override float Spread => -0.05f;
    public override float MoveSpeed => 0.05f;
    public override float WeaponDamage => -0.05f;
    public override float FireRate => -0.10f;
}

public class Suppressor : GunMod
{
    public override string Name => "Suppressor";
    public override string Description => "+Accuracy, -Damage, +Weight";
    public override GunModCategory Category => GunModCategory.Muzzle;

    public override float WeaponDamage => -0.10f;
    public override float MoveSpeed => -0.10f;
    public override float Recoil => -0.10f;
    public override float Spread => -0.10f;
}

public class MuzzleBreak : GunMod
{
    public override string Name => "Muzzle Break";
    public override string Description => "+Accuracy, +Weight, -Fire Rate";
    public override GunModCategory Category => GunModCategory.Muzzle;

    public override float Recoil => -0.20f;
    public override float MoveSpeed => -0.05f;
    public override float FireRate => -0.15f;
}

public class Compensator : GunMod
{
    public override string Name => "Compensator";
    public override string Description => "+Accuracy, +Fire Rate, +Weight";
    public override GunModCategory Category => GunModCategory.Muzzle;

    public override float Recoil => -0.10f;
    public override float MoveSpeed => -0.15f;
    public override float FireRate => 0.10f;
}
