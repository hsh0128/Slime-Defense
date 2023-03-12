using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRemoveTestButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        InGameManager.instance.OnTurretRemoveButtonPressed();
    }
}
