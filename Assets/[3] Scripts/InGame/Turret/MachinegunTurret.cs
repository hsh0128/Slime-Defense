public class MachinegunTurret : ProjectileTurretModel
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxCooldown = 0.15f;
        _turretidx = 5;
        _type = TurretType.D;
    }
}
