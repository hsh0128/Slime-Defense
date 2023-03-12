using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabySlime : SlimeModel
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxHp = 650f;
        hp = 650f;
        moveSpeed = 3f;
        reward = 0;
    }
}
