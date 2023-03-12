public class ElecSlime : SlimeModel
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxHp = 1000f;
        hp = 1000f;
        moveSpeed = 3f;
        reward = 0;
    }

    public override void AddBuff(BuffInfo buff)
    {
        buff.remainTime /= 2f;

        base.AddBuff(buff);
    }
}
