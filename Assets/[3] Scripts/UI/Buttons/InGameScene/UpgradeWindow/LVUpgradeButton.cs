using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVUpgradeButton : ButtonModel
{
    public int index;

    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        UpgradeManager.instance.OnUpgradeButtonClicked(index);
    }
}
