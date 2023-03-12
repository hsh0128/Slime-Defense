using System.Collections.Generic;
using UnityEngine;

public class TreeTurret : EnergyAttackTurretModel
{
    [SerializeField]
    protected GameObject _damageEffect;
    [SerializeField]
    protected float _damage;

    protected override void InitStatus()
    {
        base.InitStatus();

        maxCooldown = 1f;
        _turretidx = 10;
        _type = TurretType.B;
    }

    protected override void NormalShoot()
    {
        base.NormalShoot();

        EntityModel tar = nowTarget.GetComponent<EntityModel>();

        Attack(tar);

        if (anim != null) anim.ShootAnimation();
    }

    protected override void EnergyShoot()
    {
        base.EnergyShoot();

        List<GameObject> cache = new List<GameObject>();

        foreach(GameObject g in _ranges.targets)
        {
            cache.Add(g);
        }

        foreach (GameObject g in cache)
        {
            EntityModel tar = g.GetComponent<EntityModel>();

            Attack(tar);
        }

        if (anim != null) anim.ShootAnimation();
    }

    private void Attack(EntityModel t)
    {
        if (_damageEffect != null)
        {
            GameObject eff = Instantiate(_damageEffect, t.transform.position, new Quaternion());
            HitEffectModel effData = eff.GetComponent<HitEffectModel>();

            effData.attachTransform = t.transform;
        }

        t.GetDamage(_damage, DamageType.HITSCAN);
    }
}
