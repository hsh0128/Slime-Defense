using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AttackAreaStayModel : MonoBehaviour
{
    public float lifeTime;
    public float damage;
    public float attackDelay;

    private float _remain;
    private List<AttackedSlimeData> _slimes;

    private void Awake()
    {
        _remain = lifeTime;
        _slimes = new List<AttackedSlimeData>();

        transform.localScale = new Vector3(transform.localScale.x * UpgradeManager.instance.areaSizeMultiplier,
                                           transform.localScale.y * UpgradeManager.instance.areaSizeMultiplier,
                                           transform.localScale.z);
    }

    private void Update()
    {
        _remain -= Time.deltaTime;

        if (_remain <= 0) Destroy(gameObject);

        foreach(AttackedSlimeData i in _slimes)
        {
            i.remain -= Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;

        AttackedSlimeData data = null;

        foreach (AttackedSlimeData i in _slimes)
        {
            if (i.slimeObject == go)
            {
                data = i;
                break;
            }
        }

        if (data == null)
        {
            data = new AttackedSlimeData(0f, go.GetComponent<SlimeModel>(), go);
            _slimes.Add(data);
        }

        if (data.remain <= 0f && data.slime != null)
        {
            data.slime.GetDamage(damage, DamageType.AREA);
            OnGiveDamage(data.slime);
            data.remain = attackDelay;
        }
    }

    protected virtual void OnGiveDamage(SlimeModel target)
    {
        
    }
}
