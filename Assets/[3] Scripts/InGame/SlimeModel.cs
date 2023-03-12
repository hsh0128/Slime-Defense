using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// rigidbody2D는 Kinetic으로 설정하세요
/// </summary>
[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class SlimeModel : EntityModel
{
    #region Variable
    private const float CAP_LENGTH = 0.05f;

    private float _moveSpeed;
    protected float _slowResist;
    protected int _nowPositionIndex;
    private int _reward;

    private List<Vector2> _path;

    public int reward
    {
        get
        {
            return _reward;
        }
        set
        {
            _reward = value;
        }
    }

    public float moveSpeed
    {
        get
        {
            return _moveSpeed;
        }
        set
        {
            _moveSpeed = value;
        }
    }
    #endregion

    protected override void PreInit()
    {
        _path = InGameManager.instance.slimePath;
        _nowPositionIndex = 0;
        _slowResist = 0f;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        OnSlimeUpdate();

    }

    protected virtual void OnSlimeUpdate()
    {
        if (_isDead) return;

        if (_nowPositionIndex == _path.Count)
        {
            InGameManager.instance.GiveDamageToNexus(hp);
            WaveManager.instance.OnSlimeDead();

            Destroy(gameObject);
            return;
        }

        if (Vector2.Distance(_path[_nowPositionIndex], transform.position) < CAP_LENGTH)
        {
            _nowPositionIndex += 1;
            return;
        }

        float speedMultiplier = 1f;

        foreach (BuffInfo buff in buffs)
        {
            float multiplier = 1f - ((1f - buff.SlowMultiplier()) * (1f - _slowResist));

            //speedMultiplier *= buff.SlowMultiplier();
            speedMultiplier *= multiplier;
        }

        transform.position += (Vector3)(_path[_nowPositionIndex] - (Vector2)transform.position).normalized * moveSpeed * speedMultiplier * Time.deltaTime;
    }

    protected virtual void OnSlimeDead()
    {
        Debug.Log("슬라임 사망");

        InGameManager.instance.GetCoin(_reward);
        WaveManager.instance.OnSlimeDead();
        ArchieveManager.instance.SlimeKillObserve();
    }

    protected override void OnDead()
    {
        base.OnDead();

        OnSlimeDead();
    }
}
