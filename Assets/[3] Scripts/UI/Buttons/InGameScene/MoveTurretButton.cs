public class MoveTurretButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        InGameManager.instance.OnTurretMoveButtonPressed();
    }
}
