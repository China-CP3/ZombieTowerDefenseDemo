using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class TipPanel : BasePanel
{
    public Text txtInfo;
    public Button btnSure;
    public override void Init()
    {
        btnSure.onClick.AddListener(() =>
        {
            UiMgr.Instance.HidePanel<TipPanel>();   
        });
    }
    public void ChangeInfo(string str)
    {
        txtInfo.text = str;
    }
}
