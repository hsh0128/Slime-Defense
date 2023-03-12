using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInfo
{
    public float remainTime;
    public EntityModel owner;

    public virtual void OnUpdate()
    {
        remainTime -= Time.deltaTime;
    }

    public virtual float SlowMultiplier()
    {
        return 1f;
    }
}
