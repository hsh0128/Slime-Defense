using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCraftButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        InGameManager.instance.OnTurretCraftButtonPressed();
    }
}
