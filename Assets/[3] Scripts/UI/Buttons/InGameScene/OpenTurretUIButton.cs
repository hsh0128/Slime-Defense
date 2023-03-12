public class OpenTurretUIButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        InGameManager.instance.ToggleMenuUI();
    }
}
