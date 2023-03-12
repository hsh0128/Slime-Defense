using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageDownButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        ArchieveManager.instance.ArchivePageDown();
    }
}
