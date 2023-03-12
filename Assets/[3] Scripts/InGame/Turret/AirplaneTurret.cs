using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneTurret : EnergyAttackTurretModel
{
    [SerializeField]
    protected GameObject _normalProjectile;
    [SerializeField]
    protected GameObject _missileProjectile;

    protected override void InitStatus()
    {
        base.InitStatus();

        maxCooldown = 0.5f;
        _turretidx = 12;
        _type = TurretType.A;
    }

    protected override void NormalShoot()
    {
        base.NormalShoot();

        GameObject proj = Instantiate(_normalProjectile, transform.position, new Quaternion());

        ProjectileModel projInfo = proj.GetComponent<ProjectileModel>();
        projInfo.target = nowTarget;
        projInfo.startTargetPos = nowTarget.transform.position;

        if (anim != null) anim.ShootAnimation();
    }

    protected override void EnergyShoot()
    {
        base.EnergyShoot();

        GameObject proj1 = Instantiate(_missileProjectile, transform.position, new Quaternion());
        GameObject proj2 = Instantiate(_missileProjectile, transform.position, new Quaternion());

        MissileProjectile projInfo1 = proj1.GetComponent<MissileProjectile>();
        MissileProjectile projInfo2 = proj2.GetComponent<MissileProjectile>();
        projInfo1.target = nowTarget;
        projInfo1.startPos = transform.position;
        projInfo2.target = nowTarget;
        projInfo2.startPos = transform.position;
        projInfo1.targetInitialPos = nowTarget.transform.position;
        projInfo2.targetInitialPos = nowTarget.transform.position;

        Vector2 targetDirection = (nowTarget.transform.position - transform.position).normalized;

        projInfo1.dPos = transform.position + Quaternion.AngleAxis(-90, Vector3.forward) * targetDirection;
        projInfo2.dPos = transform.position + Quaternion.AngleAxis(90, Vector3.forward) * targetDirection;

        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;

        projInfo1.transform.eulerAngles = Vector3.forward * angle;
        projInfo2.transform.eulerAngles = Vector3.forward * angle;
    }
}
