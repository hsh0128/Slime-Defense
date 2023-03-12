using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTurret : HitscanTurret
{
    public GameObject gravityField;

    protected override void InitStatus()
    {
        base.InitStatus();

        maxCooldown = 5f;
        _turretidx = 11;
        _type = TurretType.A;

        _applyAtkSpeedUpgrade = false;
    }

    protected override void OnGiveDamage(EntityModel t)
    {
        base.OnGiveDamage(t);

        Instantiate(gravityField, t.transform.position, new Quaternion());
    }
}
