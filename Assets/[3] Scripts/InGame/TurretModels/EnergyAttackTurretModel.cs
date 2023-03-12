using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyAttackTurretModel : AttackTurretModel
{
    public Image EnergyBar;

    public float energyOnAttackAmount;

    [SerializeField]
    protected float _maxEnergy;

    private float _nowEnergy;

    public float nowEnergy
    {
        get
        {
            return _nowEnergy;
        }
        set
        {
            _nowEnergy = value;

            if (_nowEnergy > _maxEnergy) _nowEnergy = _maxEnergy;

            if (EnergyBar is Image)
            {
                EnergyBar.fillAmount = _nowEnergy / _maxEnergy;
            }
        }
    }

    public float maxEnergy
    {
        get
        {
            return _maxEnergy;
        }
        set
        {
            _maxEnergy = value;
        }
    }

    protected override void Shoot()
    {
        base.Shoot();

        if (nowEnergy >= maxEnergy)
        {
            nowEnergy = 0;
            EnergyShoot();
        } else
        {
            nowEnergy += energyOnAttackAmount * UpgradeManager.instance.energyMutliplier;
            NormalShoot();
        }
    }

    protected virtual void NormalShoot()
    {
        
    }

    protected virtual void EnergyShoot()
    {
        
    }
}
