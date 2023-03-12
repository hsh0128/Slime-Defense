using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTurretUIButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        InGameManager.instance.CloseMenuUI();
    }
}
