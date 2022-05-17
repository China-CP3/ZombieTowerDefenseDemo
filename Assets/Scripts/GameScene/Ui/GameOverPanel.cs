using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOverPanel : BasePanel
{
    public Text txtWinOrLose;
    public Text txtMoneyNum;
    public Button btnSure;
    public override void Init()
    {
        btnSure.onClick.AddListener(() =>
        {
            UiMgr.Instance.HidePanel<GameOverPanel>();
            UiMgr.Instance.HidePanel<GamePanel>();
            //切换场景
            SceneManager.LoadScene("BeginScene");
            GameLevelMgr.Instance.ClearInfo();
        });
    }
    public void InitInfo(int money,bool isWin)
    {
        txtWinOrLose.text = isWin ? "玩家胜利！" : "遗憾败北！";
        txtMoneyNum.text = money.ToString();
        GameDataMgr.Instance.playerData.GoldNum+=money;
        GameDataMgr.Instance.SavePlayerData();
    }
    public override void ShowMe()
    {
        base.ShowMe();
        Cursor.lockState=CursorLockMode.None;//弹出游戏结束面板后 解锁鼠标
    }
}
