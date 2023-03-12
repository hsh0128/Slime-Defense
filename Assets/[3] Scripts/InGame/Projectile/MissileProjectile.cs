using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MissileProjectile : MonoBehaviour
{
    private readonly float CAP_LENGTH = 0.2f;

    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public Vector3 dPos;
    [HideInInspector]
    public Vector3 startPos;
    [HideInInspector]
    public Vector3 targetInitialPos;

    public float damage;
    public float speed;
    public float endTime;
    public GameObject HitEffect;

    private Vector3 _cachePos;
    private float _timer;
    private bool _isChasing;

    private void Awake()
    {
        _timer = 0;
        _isChasing = false;
    }

    private void Start()
    {
        _cachePos = targetInitialPos;
    }

    private void Update()
    {
        if (target != null)
        {
            _cachePos = target.transform.position;
        }
        
        _timer += Time.deltaTime;

        if (!_isChasing)
        {
            if (_timer >= endTime)
            {
                transform.position = dPos;
                _isChasing = true;

                return;
            }

            transform.position = Vector3.Lerp(startPos, dPos, _timer / endTime);

            return;
        }

        if (target == null)
        {
            transform.position += (_cachePos - transform.position).normalized * Time.deltaTime * speed;

            if (Vector2.Distance((Vector2)_cachePos, (Vector2)transform.position) < CAP_LENGTH)
            {
                OnDummyHit(_cachePos);
            }

            return;
        }

        _cachePos = target.transform.position;

        transform.position += (_cachePos - transform.position).normalized * Time.deltaTime * speed;

        Vector2 direction = _cachePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        transform.eulerAngles = Vector3.forward * angle;

        if (Vector2.Distance((Vector2)_cachePos, (Vector2)transform.position) < CAP_LENGTH)
        {
            EntityModel info = target.GetComponent<EntityModel>();

            info.GetDamage(damage, DamageType.PROJECTILE);
            OnHit(info);
        }
    }

    protected virtual void OnDummyHit(Vector3 pos)
    {
        Instantiate(HitEffect, transform.position, new Quaternion());

        Destroy(gameObject);
    }

    protected virtual void OnHit(EntityModel target)
    {
        Instantiate(HitEffect, transform.position, new Quaternion());

        Destroy(gameObject);
    }
}
