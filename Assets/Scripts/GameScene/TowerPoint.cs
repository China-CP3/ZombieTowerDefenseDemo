using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPoint : MonoBehaviour
{
    private GameObject towerObj=null;//塔预制体
    public TowerInfo nowTowerInfo=null;//塔数据
    public List<int> chooseIds;//可以建造的3个塔id
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if(nowTowerInfo!=null&&nowTowerInfo.next==0)//防御塔已经满级 不用显示造塔升级界面
        {
            return;
        }
        UiMgr.Instance.GetPanel<GamePanel>().UpdateTowerPoint(this);
    }
    private void OnTriggerExit(Collider other)
    {
        UiMgr.Instance.GetPanel<GamePanel>().UpdateTowerPoint(null);

    }
    public void CreateTower(int id)
    {
        TowerInfo info=GameDataMgr.Instance.towerInfoList[id-1];
        if(info.money>GameLevelMgr.Instance.playerObejct.money)//买不起塔
        {
            return;
        }
        else//买得起塔
        {
            GameLevelMgr.Instance.playerObejct.AddMoney(-1*info.money);
            if(towerObj!=null)
            {
                Destroy(towerObj);
                towerObj = null;
            }
            towerObj= Instantiate(Resources.Load<GameObject>(info.res), this.transform.position, Quaternion.identity);
            towerObj.GetComponent<TowerObj>().InitInfo(info);
            nowTowerInfo = info;
            if(info.next!=0)
            {
                UiMgr.Instance.GetPanel<GamePanel>().UpdateTowerPoint(this);//更新塔按钮
            }
            else
            {
                UiMgr.Instance.GetPanel<GamePanel>().UpdateTowerPoint(null);
            }
        }

    }
}
