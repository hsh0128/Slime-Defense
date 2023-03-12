using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenArchiveWindowButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        ArchieveManager.instance.OpenArchiveWindow();
    }
}
