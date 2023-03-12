using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class TurretRangeModel : MonoBehaviour
{
    private readonly float RANGE_COLOR_ALPHA = 0.3f;

    [HideInInspector]
    public List<GameObject> targets;
    public CircleCollider2D rangeCollider;

    [SerializeField]
    private AttackTurretModel _turretBase;

    private SpriteRenderer _rangeSprite;

    private void Awake()
    {
        targets = new List<GameObject>();
        _rangeSprite = GetComponent<SpriteRenderer>();

        _rangeSprite.color = new Color(_rangeSprite.color.r, _rangeSprite.color.g, _rangeSprite.color.b, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!targets.Contains(collision.gameObject))
            targets.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (targets.Contains(collision.gameObject))
        {
            targets.Remove(collision.gameObject);

            if (_turretBase.nowTarget == collision.gameObject)
            {
                _turretBase.nowTarget = null;
            }
        }
    }

    public void OnSelected()
    {
        _rangeSprite.color = new Color(_rangeSprite.color.r, _rangeSprite.color.g, _rangeSprite.color.b, RANGE_COLOR_ALPHA);
    }

    public void OnDeselected()
    {
        _rangeSprite.color = new Color(_rangeSprite.color.r, _rangeSprite.color.g, _rangeSprite.color.b, 0f);
    }
}
