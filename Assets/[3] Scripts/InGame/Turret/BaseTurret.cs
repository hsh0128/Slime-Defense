using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTurret : ProjectileTurretModel
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxCooldown = 0.75f;
        _turretidx = 0;
        _type = TurretType.D;
    }
}
