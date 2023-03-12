using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSelectedTurretUIButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        InGameManager.instance.ToggleUI();
    }
}
