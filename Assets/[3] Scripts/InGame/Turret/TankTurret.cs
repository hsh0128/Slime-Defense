using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTurret : ProjectileTurretModel
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxCooldown = 1f;
        _turretidx = 2;
        _type = TurretType.B;
    }
}
