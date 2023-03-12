public class ImpTurret : NonTargetTurret
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxCooldown = 1f;
        _turretidx = 7;
        _type = TurretType.C;
    }
}
