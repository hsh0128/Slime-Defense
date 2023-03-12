using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedBabySlime : SlimeModel
{
    [HideInInspector]
    public int motherSlimeIndex;

    private void Start()
    {
        _nowPositionIndex = motherSlimeIndex;
    }

    protected override void InitStatus()
    {
        base.InitStatus();

        maxHp = 650f;
        hp = 650f;
        moveSpeed = 3f;
        reward = 0;
    }

    protected override void OnSlimeDead()
    {
        // ������ ���� X & Manager�鿡�� Notice �Ѹ��� ����
        //base.OnSlimeDead();
    }
}
