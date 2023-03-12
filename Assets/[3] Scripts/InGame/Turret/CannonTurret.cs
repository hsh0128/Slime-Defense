public class CannonTurret : ProjectileTurretModel
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxCooldown = 1.5f;
        _turretidx = 6;
        _type = TurretType.D;
    }
}
