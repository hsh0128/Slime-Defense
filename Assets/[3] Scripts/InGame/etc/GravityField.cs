using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : AttackAreaStayModel
{
    protected override void OnGiveDamage(SlimeModel target)
    {
        base.OnGiveDamage(target);

        target.AddBuff(new SlowBuff(0.5f, target, 0.4f));
    }
}
