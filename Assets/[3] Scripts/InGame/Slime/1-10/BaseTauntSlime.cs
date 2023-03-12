using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTauntSlime : SlimeModel
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxHp = 125f;
        hp = 125f;
        moveSpeed = 3f;
        reward = 0;
    }
}
