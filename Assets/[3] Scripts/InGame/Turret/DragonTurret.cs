public class DragonTurret : NonTargetTurret
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxCooldown = 2f;
        _turretidx = 9;
        _type = TurretType.A;
    }

    protected override void PreInit()
    {
        base.PreInit();

        anim.UpdateRotation(180f);
    }
}
