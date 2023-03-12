using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileModel : MonoBehaviour
{
    private readonly float CAP_LENGTH = 0.2f;

    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public Vector3 startTargetPos;

    public float damage;
    public float speed;
    public bool rotateToTarget;

    private Vector3 _cachePos;

    private void Start()
    {
        _cachePos = startTargetPos;
    }

    private void Update()
    {
        if (target == null)
        {
            transform.position += (_cachePos - transform.position).normalized * Time.deltaTime * speed;

            if (rotateToTarget)
            {
                Vector2 direction = _cachePos - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

                transform.eulerAngles = Vector3.forward * angle;
            }

            if (Vector2.Distance((Vector2)_cachePos, (Vector2)transform.position) < CAP_LENGTH)
            {
                OnDummyHit(_cachePos);
            }

            return;
        }

        _cachePos = target.transform.position;

        transform.position += (_cachePos - transform.position).normalized * Time.deltaTime * speed;

        if (rotateToTarget)
        {
            Vector2 direction = _cachePos - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

            transform.eulerAngles = Vector3.forward * angle;
        }

        if (Vector2.Distance((Vector2)_cachePos, (Vector2)transform.position) < CAP_LENGTH)
        {
            EntityModel info = target.GetComponent<EntityModel>();

            info.GetDamage(damage, DamageType.PROJECTILE);
            OnHit(info);
        }
    }

    /// <summary>
    /// GameObject에 접근하고 싶다면(그럴일은 없겠지만...transform 말고 쓸일 있나) 'target' 필드에 접근
    /// </summary>
    /// <param name="targetInfo"></param>
    protected virtual void OnHit(EntityModel targetInfo)
    {
        Destroy(gameObject);
    }

    protected virtual void OnDummyHit(Vector3 targetPos)
    {
        Destroy(gameObject);
    }
}
