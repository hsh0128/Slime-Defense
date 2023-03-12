using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyPowerSlime : SlimeModel
{
    private readonly string SLIMELING_NAME = "SummonedBabySlime";

    public Animator slimeAnimator;

    private Object _babySlimeData;

    protected override void PreInit()
    {
        base.PreInit();

        _babySlimeData = Resources.Load("Slimes/" + SLIMELING_NAME);
    }

    protected override void InitStatus()
    {
        base.InitStatus();

        maxHp = 800f;
        hp = 800f;
        moveSpeed = 3f;
        reward = 0;
    }

    protected override IEnumerator OnDeadCoroutine()
    {
        slimeAnimator.SetTrigger("OnDead");

        yield return new WaitForSeconds(0.2f);

        SummonedBabySlime s1 = ((GameObject)Instantiate(_babySlimeData, transform.position, new Quaternion())).GetComponent<SummonedBabySlime>();
        s1.motherSlimeIndex = _nowPositionIndex;

        yield return new WaitForSeconds(0.2f);

        SummonedBabySlime s2 = ((GameObject)Instantiate(_babySlimeData, transform.position, new Quaternion())).GetComponent<SummonedBabySlime>();
        s2.motherSlimeIndex = _nowPositionIndex;

        yield return new WaitForSeconds(0.2f);

        SummonedBabySlime s3 = ((GameObject)Instantiate(_babySlimeData, transform.position, new Quaternion())).GetComponent<SummonedBabySlime>();
        s3.motherSlimeIndex = _nowPositionIndex;

        yield return base.OnDeadCoroutine();
    }
}
