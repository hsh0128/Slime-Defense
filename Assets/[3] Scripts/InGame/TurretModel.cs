using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretModel : EntityModel
{
    #region Variables
    [SerializeField]
    protected GameObject _turretHead;
    [SerializeField]
    protected TurretHeadAnimation anim;

    private SpriteRenderer _baseSprite;

    protected int _turretidx;
    protected TurretType _type;

    private int _gridX, _gridY;

    public int gridX
    {
        get
        {
            return _gridX;
        }
    }

    public int gridY
    {
        get
        {
            return _gridY;
        }
    }

    public int turretIndex
    {
        get
        {
            return _turretidx;
        }
    }

    public TurretType type
    {
        get
        {
            return _type;
        }
    }
    #endregion

    protected override void PreInit()
    {
        base.PreInit();

        _baseSprite = GetComponent<SpriteRenderer>();
    }

    public virtual void OnSelected()
    {
        _baseSprite.sprite = AssetManager.instance.GetSelectedTurretBase(_type);
    }

    public virtual void OnDeselected()
    {
        _baseSprite.sprite = AssetManager.instance.deselectedTurretBase;
    }

    protected virtual void OnShoot()
    {

    }

    public void InitPosition(int x, int y)
    {
        _gridX = x;
        _gridY = y;
    }
}
