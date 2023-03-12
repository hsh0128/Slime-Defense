using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AttackAreaModel : MonoBehaviour
{
    public float lifetime;
    public float damage;

    private float _remain;
    private List<GameObject> _attackedSlime;

    private void Awake()
    {
        _remain = lifetime;
        _attackedSlime = new List<GameObject>();

        transform.localScale = new Vector3(transform.localScale.x * UpgradeManager.instance.areaSizeMultiplier,
                                           transform.localScale.y * UpgradeManager.instance.areaSizeMultiplier,
                                           transform.localScale.z);
    }

    private void Update()
    {
        _remain -= Time.deltaTime;

        if (_remain <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_attackedSlime.Contains(collision.gameObject)) return;

        _attackedSlime.Add(collision.gameObject);

        SlimeModel slime = collision.gameObject.GetComponent<SlimeModel>();

        if (slime == null) return;

        slime.GetDamage(damage, DamageType.AREA);
    }
}
