using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTurret : HitscanTurret
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxCooldown = 1f;
        _turretidx = 3;
        _type = TurretType.B;
    }

    protected override void OnGiveDamage(EntityModel t)
    {
        base.OnGiveDamage(t);

        t.AddBuff(new StunBuff(0.25f, t));
    }
}
