using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseArchiveWindowButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        ArchieveManager.instance.CloseArchiveWindow();
    }
}
