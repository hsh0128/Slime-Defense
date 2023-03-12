using System.Collections;
using UnityEngine;

public class HitscanTurret : AttackTurretModel
{
    [SerializeField]
    protected GameObject _damageEffect;
    [SerializeField]
    protected float _damage;
    [SerializeField]
    protected float _delay;

    protected override void Shoot()
    {
        base.Shoot();

        EntityModel tar = nowTarget.GetComponent<EntityModel>();

        StartCoroutine(ShootCoroutine(tar));

        if (anim != null) anim.ShootAnimation();
    }

    private IEnumerator ShootCoroutine(EntityModel t)
    {
        yield return new WaitForSeconds(_delay);

        if (t == null)
            yield break;

        if (_damageEffect != null)
        {
            GameObject eff = Instantiate(_damageEffect, t.transform.position, new Quaternion());
            HitEffectModel effData = eff.GetComponent<HitEffectModel>();

            effData.attachTransform = t.transform;
        }
        t.GetDamage(_damage, DamageType.HITSCAN);
        OnGiveDamage(t);

        yield return null;
    }

    protected virtual void OnGiveDamage(EntityModel t)
    {

    }
}
