using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSlime : SlimeModel
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxHp = 50f;
        hp = 50f;
        moveSpeed = 4f;
        reward = 1;
    }
}
