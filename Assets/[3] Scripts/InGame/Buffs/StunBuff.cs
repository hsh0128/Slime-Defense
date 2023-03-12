public class StunBuff : BuffInfo
{
    public StunBuff(float remainTime, EntityModel owner)
    {
        this.remainTime = remainTime;
        this.owner = owner;
    }

    public override float SlowMultiplier()
    {
        return 0f;
    }
}
