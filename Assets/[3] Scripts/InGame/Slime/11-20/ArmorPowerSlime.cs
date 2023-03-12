public class ArmorPowerSlime : SlimeModel
{
    private readonly float PIVOT_CONST =200f;

    protected override void InitStatus()
    {
        base.InitStatus();

        maxHp = 600f;
        hp = 600f;
        moveSpeed = 2.5f;
        reward = 0;
    }

    public override void GetDamage(float damage, DamageType dtype)
    {
        float percent = damage / PIVOT_CONST < 0.5f ? 0.5f : damage / PIVOT_CONST;
        if (percent > 1f) percent = 1f;

        base.GetDamage(damage * percent, dtype);
    }
}
