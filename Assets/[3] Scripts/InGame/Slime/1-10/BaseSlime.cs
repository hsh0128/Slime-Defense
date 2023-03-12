public class BaseSlime : SlimeModel
{
    protected override void InitStatus()
    {
        base.InitStatus();

        maxHp = 50f;
        hp = 50f;
        moveSpeed = 3f;
        reward = 0;
    }
}
