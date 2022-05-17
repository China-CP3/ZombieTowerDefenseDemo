using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseScenePanel : BasePanel
{
    public Button btnLeft;
    public Button btnRight;
    public Button btnStart;
    public Button btnBack;

    public Text txtInfo;
    public Image imgScene;

    private SceneInfo nowSceneInfo;
    private int nowIndex;
    public override void Init()
    {

        btnLeft.onClick.AddListener(() =>
        {
            if(nowIndex==0)
            {
                nowIndex = GameDataMgr.Instance.sceneInfoList.Count - 1;
            }
            else
            {
                nowIndex--;
            }
            ChangeScene();
        });
        btnRight.onClick.AddListener(() =>
        {
            if (nowIndex == GameDataMgr.Instance.sceneInfoList.Count - 1)
            {
                nowIndex = 0;
            }
            else
            {
                nowIndex++;
            }
            ChangeScene();
        });
        btnStart.onClick.AddListener(() =>
        {
            UiMgr.Instance.HidePanel<ChooseScenePanel>();
            //打开游戏场景
            AsyncOperation ao=SceneManager.LoadSceneAsync(nowSceneInfo.sceneName);//保证在场景完全加载完毕后，再去初始化下个场景，否则有可能会在场景没加载完时，去找下个场景的对象就会报错。
            ao.completed += (obj) =>
            {
                GameLevelMgr.Instance.InitInfo(nowSceneInfo);
            };           
        });
        btnBack.onClick.AddListener(() =>
        {
            UiMgr.Instance.HidePanel<ChooseScenePanel>();
            UiMgr.Instance.ShowPanel<ChooseHeroPanel>();
        });
        ChangeScene();
    }
    public void ChangeScene()
    {
        nowSceneInfo = GameDataMgr.Instance.sceneInfoList[nowIndex];
        imgScene.sprite = Resources.Load<Sprite>(nowSceneInfo.imgRes);
        txtInfo.text = "名称:\n" + nowSceneInfo.name +"\n"+"描述："+"\n" + nowSceneInfo.tips;
    }
}
