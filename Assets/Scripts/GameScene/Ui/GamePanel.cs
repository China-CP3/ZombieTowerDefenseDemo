using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GamePanel : BasePanel
{
    private bool checkInput=false;
    public GameObject objSetting;
    public GameObject objBot;

    public bool IsSettingBtnOpen;

    public Image imgSettingBk;
    public Image imgHp;

    public Text txtHp;
    public Text txtWave;
    public Text txtGoldNum;

    public Button btnSetting;
    public Button btnStop;
    public Button btnContinue;
    public Button btnQuit;
    public Button btnCancel;
    
    public List<BtnTower> towerBtns=new List<BtnTower>();//防御塔的脚本

    public TowerPoint nowSelTowerPoint;

    public override void Init()
    {
        Cursor.lockState = CursorLockMode.Confined;//一开始就锁定鼠标 不然鼠标会跑到窗口外
        imgSettingBk.gameObject.SetActive(false);
        objSetting.gameObject.SetActive(false);
        //objBot.gameObject.SetActive(false);

        btnSetting.onClick.AddListener(() =>
        {
            if(IsSettingBtnOpen)
            {
                imgSettingBk.gameObject.SetActive(false);
                objSetting.gameObject.SetActive(false);
                IsSettingBtnOpen = false;
            }
            else
            {              
                imgSettingBk.gameObject.SetActive(true);
                objSetting.gameObject.SetActive(true);
                IsSettingBtnOpen = true;
            }
        });
        btnStop.onClick.AddListener(() =>
        {
            Time.timeScale = 0;
        });
        btnContinue.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
        });
        btnQuit.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            UiMgr.Instance.HidePanel<GamePanel>();
            GameLevelMgr.Instance.ClearInfo();
            SceneManager.LoadScene("BeginScene");
        });
        btnCancel.onClick.AddListener(() =>
        {
            imgSettingBk.gameObject.SetActive(false);
            objSetting.gameObject.SetActive(false);
            IsSettingBtnOpen = false;
        });
    }
    /// <summary>
    /// 更新血量
    /// </summary>
    /// <param name="hp"></param>
    public void UpdateTowerHp(int hp)
    {
        imgHp.fillAmount = hp*0.01f;
        txtHp.text = hp.ToString();

    }
    /// <summary>
    /// 更新当前波数和最大波数
    /// </summary>
    /// <param name="nowNum"></param>
    /// <param name="maxNum"></param>
    public void UpdateWaveNum(int nowNum,int maxNum)
    {
        txtWave.text= nowNum+"/"+maxNum;

    }
    /// <summary>
    /// 更新金币
    /// </summary>
    public void UpdateMoney(int money)
    {
        txtGoldNum.text= money.ToString();
    }
    /// <summary>
    /// 更新当前选中的造塔点界面的一些变化
    /// </summary>
    public void UpdateTowerPoint(TowerPoint towerPoint)
    {
        if(towerPoint==null)
        {
            objBot.gameObject.SetActive(false);
            checkInput = false;
        }
        else
        {
            checkInput = true;
            objBot.gameObject.SetActive(true);
            //根据造塔点的信息 决定界面上的显示内容
            nowSelTowerPoint = towerPoint;
            if (nowSelTowerPoint.nowTowerInfo == null)//塔数据为空 说明这个造塔点还没造塔
            {
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(true);
                    towerBtns[i].InitInfo(nowSelTowerPoint.chooseIds[i], "数字键" + (i + 1));
                }
            }
            else
            {
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(false);
                }
                towerBtns[1].gameObject.SetActive(true);
                towerBtns[1].InitInfo(towerPoint.nowTowerInfo.next, "空格键");
            }
        }
        
    }
    private void Update()
    {
        base.Update();
        if (!checkInput)
            return;
        //造塔点  键盘输入 造塔
        if (nowSelTowerPoint.nowTowerInfo==null)//info是从json读取的塔数据  在造塔点里购买的  如果买不起就为空  买了才不为空   为空说明从没买过塔
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIds[0]);//chooseIds里面是写死的id 1,4,7 从json里读取list中第1,4,7条塔的数据
            }else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIds[1]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIds[2]);
            }
        }
        else//说明买过塔 这次是升级 
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.nowTowerInfo.next);
            }
        }
    }
}
