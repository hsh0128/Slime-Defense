public class ElecTauntSlime : SlimeModel
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxHp = 1100f;
        hp = 1100f;
        moveSpeed = 3f;
        reward = 0;
    }

    public override void AddBuff(BuffInfo buff)
    {
        buff.remainTime /= 2f;

        base.AddBuff(buff);
    }
}
