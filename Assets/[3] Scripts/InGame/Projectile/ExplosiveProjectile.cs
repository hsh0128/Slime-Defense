using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExplosiveProjectile : ProjectileModel
{
    [SerializeField]
    protected GameObject _area;

    protected override void OnHit(EntityModel targetInfo)
    {
        Instantiate(_area, targetInfo.transform.position, new Quaternion());

        base.OnHit(targetInfo);
    }

    protected override void OnDummyHit(Vector3 targetPos)
    {
        Instantiate(_area, targetPos, new Quaternion());

        base.OnDummyHit(targetPos);
    }
}
