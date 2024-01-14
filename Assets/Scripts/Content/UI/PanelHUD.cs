using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelHUD : UIWidget
{
    public TMP_Text txtPaused;

    public override void Init()
    {
        base.Init();

        SetPaused(game.paused);
    }

    public void SetPaused(bool v)
    {
        txtPaused.gameObject.SetActive(v);
    }
}
