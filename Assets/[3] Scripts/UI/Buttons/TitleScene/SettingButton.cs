using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        SettingManager.instance.OpenSettings();
    }
}
