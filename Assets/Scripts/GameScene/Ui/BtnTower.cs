using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BtnTower : MonoBehaviour
{
    public Image imgIcon;
    public Text txtTip;
    public Text txtMoney;
    public void InitInfo(int id,string inputStr)
    {
        TowerInfo info=GameDataMgr.Instance.towerInfoList[id-1];
        imgIcon.sprite=Resources.Load<Sprite>(info.imgRes);
        txtMoney.text="$"+info.money;
        txtTip.text = inputStr;
        if(info.money>GameLevelMgr.Instance.playerObejct.money)
        {
            txtMoney.text = "½ðÇ®²»×ã";
        }
        
    }
}
