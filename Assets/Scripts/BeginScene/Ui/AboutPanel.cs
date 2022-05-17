using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AboutPanel:BasePanel
{
    public Button btnClose;

    public override void Init()
    {
        btnClose.onClick.AddListener(() =>
        {
            UiMgr.Instance.HidePanel<AboutPanel>();
        });
    }
}
