public class ElecPowerSlime : SlimeModel
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxHp = 1200f;
        hp = 1200f;
        moveSpeed = 3f;
        reward = 0;
        _slowResist = 0.5f;
    }

    public override void AddBuff(BuffInfo buff)
    {
        buff.remainTime /= 2f;

        base.AddBuff(buff);
    }
}
