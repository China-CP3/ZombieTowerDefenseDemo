using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ChooseHeroPanel : BasePanel
{
    public Button btnLeft;
    public Button btnRight;
    public Button btnUnLock;
    public Button btnStart;
    public Button btnBack;
    public Text txtGoldNum;
    public Text txtUnLock;
    public Text txtName;

    private Transform heroPos;
    private GameObject heroObj;
    private int nowIndex;
    private RoleInfo nowRoleData;
    public override void Init()
    {
        txtGoldNum.text = GameDataMgr.Instance.playerData.GoldNum.ToString();
        heroPos = GameObject.Find("HeroPos").transform;
        ChangeHero();//更新选角场景中角色模型

        btnBack.onClick.AddListener(() =>
        {
            UiMgr.Instance.HidePanel<ChooseHeroPanel>();
            Camera.main.GetComponent<BeginCameraAnimator>().TurnRight(() =>
            {
                UiMgr.Instance.ShowPanel<BeginPanel>();
            });
        });
        btnStart.onClick.AddListener(() =>
        {
            //记录当前选择的角色 隐藏选角界面
            GameDataMgr.Instance.nowRoleChoose = nowRoleData;
            UiMgr.Instance.HidePanel<ChooseHeroPanel>();
            UiMgr.Instance.ShowPanel<ChooseScenePanel>();
        });
        btnLeft.onClick.AddListener(() =>
        {
            if (nowIndex == 0)
                nowIndex = GameDataMgr.Instance.roleInfoList.Count - 1;
            else
            {
                nowIndex--;
            }
            ChangeHero();
        });
        btnRight.onClick.AddListener(() =>
        {
            if (nowIndex == GameDataMgr.Instance.roleInfoList.Count - 1)
                nowIndex = 0;
            else
            {
                nowIndex++;
            }
            ChangeHero();
        });
        btnUnLock.onClick.AddListener(() =>
        {
            PlayerData data = GameDataMgr.Instance.playerData;
            if (data.GoldNum>=nowRoleData.lockMoney)
            {
                data.GoldNum -= nowRoleData.lockMoney;
                txtGoldNum.text = data.GoldNum.ToString();
                data.boughtHero.Add(nowRoleData.id);
                btnStart.gameObject.SetActive(true);
                btnUnLock.gameObject.SetActive(false); 
                GameDataMgr.Instance.SavePlayerData();
                //提示面板  提示购买成功
                UiMgr.Instance.ShowPanel<TipPanel>().ChangeInfo("购买成功");
            }
            else
            {
                UiMgr.Instance.ShowPanel<TipPanel>().ChangeInfo("金币不够");
            }

        });
    }
    private void ChangeHero()
    {
        if(heroObj!=null)//判断场景中是否已经有角色模型
        {
            Destroy(heroObj);
            heroObj = null;
        }
        nowRoleData = GameDataMgr.Instance.roleInfoList[nowIndex];//角色数据类对象
        heroObj = Instantiate(Resources.Load<GameObject>(nowRoleData.res),heroPos.position,heroPos.rotation);//加载角色模型
        Destroy(heroObj.GetComponent<PlayerObejct>());//选角界面时 暂时不需要这个脚本 否则人物会跟随鼠标旋转还有攻击
        txtName.text = nowRoleData.tips;
        UpDateLockBtn();
    }
    private void UpDateLockBtn()
    {
        //如果该角色未解锁 就显示购买按钮并且隐藏开始游戏按钮
        if(nowRoleData.lockMoney>0&&!GameDataMgr.Instance.playerData.boughtHero.Contains(nowRoleData.id))
        {
            btnUnLock.gameObject.SetActive(true);
            txtUnLock.text = "价格"+nowRoleData.lockMoney+"金币";
            btnStart.gameObject.SetActive(false);
        }
        else
        {
            btnUnLock.gameObject.SetActive(false);
            btnStart.gameObject.SetActive(true);
        }
    }
    public override void HideMe(UnityAction callBack)
    {
        base.HideMe(callBack);
        if(heroObj!=null)
        {
            DestroyImmediate(heroObj);
            heroObj = null;
        }
    }
}
