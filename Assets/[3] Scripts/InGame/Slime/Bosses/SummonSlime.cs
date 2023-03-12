using System.Collections;
using UnityEngine;

public class SummonSlime : SlimeModel
{
    private readonly float SUMMON_DELAY = 3f;
    private readonly string SLIMELING_NAME = "SummonedBabySlime";

    public Animator summonAnimator;

    private float _nowSummonDelay;
    private Status _status;
    private Object _babySlimeData;

    private enum Status
    {
        MOVE,
        SUMMONING
    }

    protected override void PreInit()
    {
        base.PreInit();

        _nowSummonDelay = SUMMON_DELAY;
        _status = Status.MOVE;
        _babySlimeData = Resources.Load("Slimes/" + SLIMELING_NAME);
    }

    protected override void OnSlimeUpdate()
    {
        if (_status == Status.SUMMONING) return;

        _nowSummonDelay -= Time.deltaTime;

        if (_nowSummonDelay > 0f)
        {
            base.OnSlimeUpdate();

            return;
        }

        _status = Status.SUMMONING;

        StartCoroutine(SummonCoroutine());
    }

    protected override void InitStatus()
    {
        base.InitStatus();

        maxHp = 50125f;
        hp = 50125f;
        moveSpeed = 3f;
        reward = 15;
    }

    private IEnumerator SummonCoroutine()
    {
        summonAnimator.SetTrigger("OnSummon");

        yield return new WaitForSeconds(0.4f);

        SummonedBabySlime s1 = ((GameObject)Instantiate(_babySlimeData, transform.position, new Quaternion())).GetComponent<SummonedBabySlime>();
        s1.motherSlimeIndex = _nowPositionIndex;

        yield return new WaitForSeconds(0.2f);

        SummonedBabySlime s2 = ((GameObject)Instantiate(_babySlimeData, transform.position, new Quaternion())).GetComponent<SummonedBabySlime>();
        s2.motherSlimeIndex = _nowPositionIndex;

        yield return new WaitForSeconds(0.2f);

        SummonedBabySlime s3 = ((GameObject)Instantiate(_babySlimeData, transform.position, new Quaternion())).GetComponent<SummonedBabySlime>();
        s3.motherSlimeIndex = _nowPositionIndex;

        yield return new WaitForSeconds(0.2f);

        _nowSummonDelay = SUMMON_DELAY;
        _status = Status.MOVE;
    }
}
