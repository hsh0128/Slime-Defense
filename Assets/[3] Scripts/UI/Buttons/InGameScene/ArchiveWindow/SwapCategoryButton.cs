using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCategoryButton : ButtonModel
{
    public int idx;

    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        ArchieveManager.instance.SwapCategory(idx);
    }
}
