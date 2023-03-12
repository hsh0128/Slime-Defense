using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTurretModel : TurretModel
{
    [SerializeField]
    protected TurretRangeModel _ranges;
    [SerializeField]
    protected bool _rotateHead;

    [HideInInspector]
    public GameObject nowTarget;

    protected bool _applyAtkSpeedUpgrade;

    private float _nowCooldown;
    private float _maxCooldown;

    public float nowCooldown
    {
        get
        {
            return _nowCooldown;
        }
        set
        {
            _nowCooldown = value;
        }
    }

    public float maxCooldown
    {
        get
        {
            return _maxCooldown;
        }
        set
        {
            _maxCooldown = value;
        }
    }

    protected override void PreInit()
    {
        base.PreInit();

        nowTarget = null;
    }

    protected override void InitStatus()
    {
        base.InitStatus();

        _applyAtkSpeedUpgrade = true;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        nowCooldown -= Time.deltaTime;

        if (nowTarget == null)
        {
            if (_ranges.targets.Count <= 0) return;

            float minLength = _ranges.rangeCollider.radius;
            float cacheLen;

            _ranges.targets.RemoveAll(x => x == null);

            foreach (GameObject tar in _ranges.targets)
            {
                cacheLen = Vector2.Distance(transform.position, tar.transform.position);

                if (minLength < cacheLen)
                {
                    minLength = cacheLen;
                    nowTarget = tar;
                }
            }
        }
        else
        {
            if (nowCooldown > 0f)
            {
                if (anim == null || _turretHead == null) return;

                UpdateHeadRotation();

                return;
            }

            if (_applyAtkSpeedUpgrade) nowCooldown = maxCooldown * UpgradeManager.instance.atkSpeedMultiplier;
            else nowCooldown = maxCooldown;

            Shoot();
            OnShoot();
        }

        if (anim == null || _turretHead == null) return;

        UpdateHeadRotation();
    }

    private void UpdateHeadRotation()
    {
        if (!_rotateHead) return;

        if (nowTarget == null) return;

        Vector2 direction = nowTarget.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        anim.UpdateRotation(angle);
    }

    public override void OnSelected()
    {
        base.OnSelected();

        _ranges.OnSelected();
    }

    public override void OnDeselected()
    {
        base.OnDeselected();

        _ranges.OnDeselected();
    }

    /// <summary>
    /// 적의 정보는 nowTarget, 나의 위치는 transform.position으로 얻기
    /// </summary>
    protected virtual void Shoot()
    {
        
    }
}
