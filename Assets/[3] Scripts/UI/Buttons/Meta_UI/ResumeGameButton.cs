using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeGameButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        GameManager.instance.ClosePauseScreen();
    }
}
