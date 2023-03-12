using UnityEngine;

public class AtomSlime : SlimeModel
{
    private readonly float PROJECTILE_INVALIDITY_PROBABILITY = 0.2f;

    public GameObject energyEffect;

    protected override void InitStatus()
    {
        base.InitStatus();

        maxHp = 85000f;
        hp = 85000f;
        moveSpeed = 3f;
        reward = 0;
        _slowResist = 0.5f;
    }

    public override void AddBuff(BuffInfo buff)
    {
        buff.remainTime /= 2;

        base.AddBuff(buff);
    }

    public override void GetDamage(float damage, DamageType dtype)
    {
        if (dtype == DamageType.PROJECTILE)
        {
            float rand = Random.Range(0f, 1f);

            if (rand < PROJECTILE_INVALIDITY_PROBABILITY)
            {
                Instantiate(energyEffect, transform.position, new Quaternion());
                return;
            }
        }

        base.GetDamage(damage, dtype);
    }
}
