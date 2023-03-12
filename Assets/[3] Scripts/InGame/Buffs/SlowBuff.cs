public class SlowBuff : BuffInfo
{
    public float slowPercent;

    public SlowBuff(float remainTime, EntityModel owner, float slowPercent)
    {
        this.remainTime = remainTime;
        this.owner = owner;
        this.slowPercent = slowPercent;
    }

    public override float SlowMultiplier()
    {
        return 1f - slowPercent;
    }
}
