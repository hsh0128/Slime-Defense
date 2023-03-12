using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUpgradeWindowButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        UpgradeManager.instance.CloseUpgradeWindow();
    }
}
