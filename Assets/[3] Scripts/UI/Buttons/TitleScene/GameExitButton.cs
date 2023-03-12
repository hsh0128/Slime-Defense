using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExitButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        Application.Quit();
    }
}
