using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonTargetTurret : AttackTurretModel
{
    [SerializeField]
    protected GameObject _area;

    protected override void Shoot()
    {
        base.Shoot();

        GameObject ar = Instantiate(_area, transform.position, new Quaternion());

        Vector2 direction = nowTarget.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        ar.transform.eulerAngles = Vector3.forward * angle;

        if (anim != null) anim.ShootAnimation();
    }
}
