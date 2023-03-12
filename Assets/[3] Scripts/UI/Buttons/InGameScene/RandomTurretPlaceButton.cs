using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTurretPlaceButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        InGameManager.instance.OnRandomTurretPlaceButtonPressed();
        InGameManager.instance.CloseMenuUI();
    }
}
