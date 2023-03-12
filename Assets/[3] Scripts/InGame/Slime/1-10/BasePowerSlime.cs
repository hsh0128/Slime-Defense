public class BasePowerSlime : SlimeModel
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxHp = 200f;
        hp = 200f;
        moveSpeed = 3f;
        reward = 0;
    }
}
