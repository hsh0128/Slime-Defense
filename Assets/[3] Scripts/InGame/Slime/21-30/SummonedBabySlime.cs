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
        // 리워드 수령 X & Manager들에게 Notice 뿌리지 않음
        //base.OnSlimeDead();
    }
}
