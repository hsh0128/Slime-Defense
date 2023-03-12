public class ArmorSlime : SlimeModel
{
    private readonly float PIVOT_CONST = 500f;

    protected override void InitStatus()
    {
        base.InitStatus();

        maxHp = 35000f;
        hp = 35000f;
        moveSpeed = 3f;
        reward = 10;
    }

    public override void GetDamage(float damage, DamageType dtype)
    {
        float percent = damage / PIVOT_CONST < 0.5f ? 0.5f : damage / PIVOT_CONST;
        if (percent > 1f) percent = 1f;

        base.GetDamage(damage * percent, dtype);
    }
}
