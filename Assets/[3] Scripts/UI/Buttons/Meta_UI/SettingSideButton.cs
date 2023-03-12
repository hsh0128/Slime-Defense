using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingSideButton : ButtonModel
{
    public int index;

    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        SettingManager.instance.OnSettingModeButtonClicked(index);
    }
}
