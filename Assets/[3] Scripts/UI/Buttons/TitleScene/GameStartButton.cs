using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        GameManager.instance.StartGame();
    }
}
