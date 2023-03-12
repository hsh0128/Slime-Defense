using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunTurret : NonTargetTurret
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxCooldown = 1f;
        _turretidx = 4;
        _type = TurretType.D;
    }
}
