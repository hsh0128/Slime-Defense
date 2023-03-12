using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipWaveDelayButton : ButtonModel
{
    protected override void OnLeftClick()
    {
        base.OnLeftClick();

        WaveManager.instance.SkipWave();
    }
}
