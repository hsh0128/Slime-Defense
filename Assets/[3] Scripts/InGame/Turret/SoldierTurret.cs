using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierTurret : ProjectileTurretModel
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxCooldown = 0.3f;
        _turretidx = 1;
        _type = TurretType.C;
    }
}
