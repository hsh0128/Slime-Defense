using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseSettingButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        SettingManager.instance.CloseSettings();
    }
}
