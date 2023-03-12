using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUpgradeWindowButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        UpgradeManager.instance.OpenUpgradeWindow();
    }
}
