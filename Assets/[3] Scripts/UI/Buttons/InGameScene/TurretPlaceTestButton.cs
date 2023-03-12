using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPlaceTestButton : ButtonModel
{
    public int index;

    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        InGameManager.instance.OnTurretPlaceButtonPressed(index);
        InGameManager.instance.CloseMenuUI();
    }
}
