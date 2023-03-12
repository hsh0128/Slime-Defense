public class BunchSlime : SlimeModel
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxHp = 9000f;
        hp = 9000f;
        moveSpeed = 3f;
        reward = 5;
    }
}
