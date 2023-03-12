using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexusModel : EntityModel
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxHp = 1000f;
        hp = 1000f;
    }

    protected override void OnDead()
    {
        base.OnDead();

        InGameManager.instance.ExecuteGameOver();
    }
}
