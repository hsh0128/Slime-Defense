using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityModel : MonoBehaviour
{
    #region Variable
    public Image HPBar;

    private SpriteRenderer _thisSprite;
    private float _hp;
    private float _maxHp;
    protected bool _isDead;

    [HideInInspector]
    public List<BuffInfo> buffs;

    public float maxHp
    {
        get
        {
            return _maxHp;
        }
        set
        {
            _maxHp = value;
        }
    }

    public float hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            if (_hp > _maxHp) _hp = _maxHp;

            if (HPBar is Image)
            {
                HPBar.fillAmount = _hp / _maxHp;
            }
        }
    }
    #endregion

    private void Awake()
    {
        _isDead = false;
        _thisSprite = GetComponent<SpriteRenderer>();

        buffs = new List<BuffInfo>();

        PreInit();
        InitStatus();
    }

    private void Update()
    {
        buffs.RemoveAll(x => x.remainTime <= 0f);

        foreach(BuffInfo buff in buffs)
        {
            buff.OnUpdate();
        }

        OnUpdate();
    }

    protected virtual void PreInit()
    {
        
    }

    /// <summary>
    /// hp, maxHp : 모두
    /// moveSpeed : 슬라임만
    /// </summary>
    protected virtual void InitStatus()
    {
        
    }

    protected virtual void OnGetDamage(float damage, DamageType dtype)
    {
        
    }

    protected virtual void OnUpdate()
    {

    }

    protected virtual void OnDead()
    {
        
    }

    protected virtual IEnumerator OnDeadCoroutine()
    {
        yield return null;

        Destroy(gameObject);
    }

    public virtual void AddBuff(BuffInfo buff)
    {
        buffs.Add(buff);
    }

    public void RemoveBuff(BuffInfo buff)
    {
        if (!buffs.Contains(buff)) return;

        buffs.Remove(buff);
    }

    public virtual void GetDamage(float damage, DamageType dtype)
    {
        if (_isDead) return;

        if (this is SlimeModel)
        {
            switch(dtype)
            {
                case DamageType.PROJECTILE:
                    damage *= UpgradeManager.instance.projDamageMultiplier;
                    break;
                case DamageType.AREA:
                    damage *= UpgradeManager.instance.areaDamageMultiplier;
                    break;
                case DamageType.HITSCAN:
                    damage *= UpgradeManager.instance.hitscanDamageMultiplier;
                    break;
                default:
                    break;
            }
        }

        hp -= damage;

        StartCoroutine(DamageEffectCoroutine());

        OnGetDamage(damage, dtype);

        if (hp <= 0) ExecuteDead();
    }

    public void ExecuteDead()
    {
        _isDead = true;

        OnDead();

        StartCoroutine(OnDeadCoroutine());
    }

    private IEnumerator DamageEffectCoroutine()
    {
        _thisSprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _thisSprite.color = Color.white;
    }
}
