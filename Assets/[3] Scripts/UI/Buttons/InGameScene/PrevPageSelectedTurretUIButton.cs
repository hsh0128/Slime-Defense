using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrevPageSelectedTurretUIButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        InGameManager.instance.nowPage -= 1;
    }
}
