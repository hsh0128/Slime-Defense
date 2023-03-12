using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageUpButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        ArchieveManager.instance.ArchivePageUp();
    }
}
