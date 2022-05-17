using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button btnStart;
    public Button btnSetting;
    public Button btnAbout;
    public Button btnQuit;
    public override void Init()
    {
        btnStart.onClick.AddListener(() =>
        {
            //播放摄像机左转动画  显示选角面板
            UiMgr.Instance.HidePanel<BeginPanel>();
            Camera.main.GetComponent<BeginCameraAnimator>().TurnLeft(() => 
            { 
                UiMgr.Instance.ShowPanel<ChooseHeroPanel>();    
            });
        });
        btnSetting.onClick.AddListener(() =>
        {
            //打开设置面板
            UiMgr.Instance.ShowPanel<SettingPanel>();
        });
        btnAbout.onClick.AddListener(() =>
        {
            //打开说明面板
            UiMgr.Instance.ShowPanel<AboutPanel>();
        });
        btnQuit.onClick.AddListener(() =>
        {
            
            //退出游戏
            Application.Quit();//打包后才生效  编辑时无效
        });
    }
}
