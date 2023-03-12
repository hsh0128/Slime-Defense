using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyTauntSlime : SlimeModel
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxHp = 800f;
        hp = 800f;
        moveSpeed = 3f;
        reward = 0;
    }
}
