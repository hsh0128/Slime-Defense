using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToTitleButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        GameManager.instance.ClosePauseScreen();
        GameManager.instance.BackToTitle();
    }
}
