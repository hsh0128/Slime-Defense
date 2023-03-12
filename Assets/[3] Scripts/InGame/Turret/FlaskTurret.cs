using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaskTurret : ProjectileTurretModel
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxCooldown = 1.5f;
        _turretidx = 8;
        _type = TurretType.C;
    }
}
