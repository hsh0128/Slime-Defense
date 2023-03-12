using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTurretModel : AttackTurretModel
{
    [SerializeField]
    protected GameObject _projectile;

    protected override void Shoot()
    {
        base.Shoot();

        GameObject proj = Instantiate(_projectile, transform.position, new Quaternion());

        ProjectileModel projInfo = proj.GetComponent<ProjectileModel>();
        projInfo.target = nowTarget;
        projInfo.startTargetPos = nowTarget.transform.position;

        if (anim != null) anim.ShootAnimation();
    }
}
